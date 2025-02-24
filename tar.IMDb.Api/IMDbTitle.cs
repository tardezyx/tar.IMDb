using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tar.IMDb.Api.Enums;
using tar.IMDb.Api.Extensions;
using tar.IMDb.Api.Wrapper;

namespace tar.IMDb.Api {
  public class IMDbTitle {
    private readonly IMDbRestClient _client = new IMDbRestClient();
    private readonly string _id;

    public delegate void OnUpdateDelegate(ProgressInfo progressInfo);
    public event OnUpdateDelegate OnUpdate;

    public List<Aka> Akas { get; set; } = new List<Aka>();
    public Awards Awards { get; set; } = new Awards();
    public List<Certificate> Certificates { get; set; } = new List<Certificate>();
    public Collection Collection { get; set; } = new Collection();
    public List<string> Countries { get; set; } = new List<string>();
    public Crew Crew { get; set; } = new Crew();
    public int? EpisodeNumber { get; set; }
    public List<string> Genres { get; set; } = new List<string>();
    public string Id { get; set; }
    public bool? IsAdult { get; set; }
    public List<string> Languages { get; set; } = new List<string>();
    public string OriginalTitle { get; set; }
    public Plot Plot { get; set; } = new Plot();
    public List<Image> Posters { get; set; } = new List<Image>();
    public List<Company> ProductionCompanies { get; set; } = new List<Company>();
    public Rating Rating { get; set; } = new Rating();
    public DateTime? ReleaseDate { get; set; }
    public List<ReleaseDate> ReleaseDates { get; set; } = new List<ReleaseDate>();
    public TimeSpan? Runtime { get; set; }
    public int? SeasonNumber { get; set; }
    public List<Season> Seasons { get; set; } = new List<Season>();
    public Title Series { get; set; }
    public Status Status { get; set; } = new Status();
    public Image StillFrame { get; set; }
    public Trailer Trailer { get; set; } = new Trailer();
    public string Type { get; set; }
    public string Url { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }

    public IMDbTitle(string titleId) {
      _id = titleId;
    }

    public async Task FetchAsync() {
      await FetchTitleAsync();

      var tasks = new List<Func<Task>> {
        FetchAkasAsync,
        FetchAwardsAsync,
        FetchCastAsync,
        FetchCertificatesAsync,
        FetchDirectorsAsync,
        FetchPlotAsync,
        FetchPostersAsync,
        FetchProductionCompaniesAsync,
        FetchReleaseDatesAsync,
        FetchWritersAsync,
        FetchSeasonsAsync,
      };

      await Task.WhenAll(
        tasks.AsParallel().Select(
          async task => await task()
        )
      );

      await FetchCollectionAsync();

      Akas = Akas
        .OrderBy(x => x.CountryId)
        .ThenBy(x => x.LanguageId)
        .ThenBy(x => x.Title)
        .ToList();

      Certificates = Certificates
        .OrderBy(x => x.CountryId)
        .ThenBy(x => x.Rating)
        .ToList();

      ReleaseDates = ReleaseDates
        .OrderBy(x => x.CountryId)
        .ThenBy(x => x.Date)
        .ToList();
    }

    private async Task FetchAkasAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchAkas);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetAkasAsync(
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var akas = response.Data.Data.Title?.Akas;

        if (akas is null) {
          break;
        }

        foreach (var aka in akas.Edges) {
          Akas.Add(
            new Aka {
              CountryId = aka.Node.Country?.Id,
              LanguageId = aka.Node.Language?.Id,
              Note = aka.Node.Attributes is null ? null : string.Join(" | ", aka.Node.Attributes.Select(x => x.Text)),
              Title = aka.Node.Text
            }
          );
        }

        previousEndCursor = akas.PageInfo.HasNextPage is true
          ? akas.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchAwardsAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchAwards);

      var response = await _client.TitleViaJson.GetAwardNominationsAsync(
        awardsFilter: AwardsFilter.NominationsOnly,
        maxNumberOfResults: 0,
        titleId: _id
      );

      if (!(response is null) && response.IsSuccessful) {
        Awards.NumberOfNominations = response.Data.Data.Title?.AwardNominations?.Total;

        progressInfo.Responses.Add(response);
        OnUpdate?.Invoke(progressInfo);
      }

      response = await _client.TitleViaJson.GetAwardNominationsAsync(
        awardsFilter: AwardsFilter.WinsOnly,
        maxNumberOfResults: 0,
        titleId: _id
      );

      if (!(response is null) && response.IsSuccessful) {
        progressInfo.Responses.Add(response);
        Awards.NumberOfWins = response.Data.Data.Title?.AwardNominations?.Total;
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchCastAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchCast);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetCreditsAsync(
          category: "cast",
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var credits = response.Data.Data.Title?.Credits;

        foreach (var credit in credits?.Edges?.EmptyIfNull()) {
          Crew.Actors.Add(
            new Person {
              Characters = credit.Node.Characters is null ? null : string.Join(" | ", credit.Node.Characters.Select(x => x.Name)),
              Id = credit.Node.Name.Id,
              ImageUrl = credit.Node.Name.PrimaryImage?.Url,
              Name = credit.Node.Name.NameText.Text,
              Url = $"https://www.imdb.com/de/name/{credit.Node.Name.Id}"
            }
          );
        }

        previousEndCursor = credits?.PageInfo?.HasNextPage is true
          ? credits.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchCertificatesAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchCertificates);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetCertificatesAsync(
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var certificates = response.Data.Data.Title?.Certificates;
        var edges = certificates?.Edges;

        foreach (var certificate in edges?.EmptyIfNull()) {
          Certificates.Add(
            new Certificate {
              CountryId = certificate.Node.Country.Id,
              Note = certificate.Node.Attributes is null ? null : string.Join(" | ", certificate.Node.Attributes.Select(x => x.Text)),
              Rating = certificate.Node.Rating
            }
          );
        }

        previousEndCursor = certificates?.PageInfo?.HasNextPage is true
          ? certificates.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchCollectionAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchCollection);

      string previousEndCursor = string.Empty;

      List<Title> preceding = new List<Title>();
      List<Title> succeeding = new List<Title>();

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetConnectionsAsync(
          category: "follows",
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var predecessors = response.Data.Data.Title?.Connections;

        foreach (var predecessor in predecessors?.Edges?.EmptyIfNull()) {
          preceding.Add(
            new Title {
              Id = predecessor.Node.AssociatedTitle.Id,
              Image = GetImage(predecessor.Node.AssociatedTitle.PrimaryImage),
              OriginalTitle = predecessor.Node.AssociatedTitle.OriginalTitleText.Text,
              Rating = GetRating(predecessor.Node.AssociatedTitle.RatingsSummary, predecessor.Node.AssociatedTitle.Metacritic),
              ReleaseDate = GetDateTime(predecessor.Node.AssociatedTitle.ReleaseDate),
              Runtime = GetRuntime(predecessor.Node.AssociatedTitle.Runtime),
              Type = predecessor.Node.AssociatedTitle.TitleType.Id,
              YearFrom = predecessor.Node.AssociatedTitle.ReleaseYear.Year,
              YearTo = predecessor.Node.AssociatedTitle.ReleaseYear.EndYear
            }
          );
        }

        previousEndCursor = predecessors?.PageInfo?.HasNextPage is true
          ? predecessors.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetConnectionsAsync(
          category: "followed_by",
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var predecessors = response.Data.Data.Title?.Connections;

        foreach (var predecessor in predecessors?.Edges?.EmptyIfNull()) {
          succeeding.Add(
            new Title {
              Id = predecessor.Node.AssociatedTitle.Id,
              Image = GetImage(predecessor.Node.AssociatedTitle.PrimaryImage),
              OriginalTitle = predecessor.Node.AssociatedTitle.OriginalTitleText.Text,
              Rating = GetRating(predecessor.Node.AssociatedTitle.RatingsSummary, predecessor.Node.AssociatedTitle.Metacritic),
              ReleaseDate = GetDateTime(predecessor.Node.AssociatedTitle.ReleaseDate),
              Runtime = GetRuntime(predecessor.Node.AssociatedTitle.Runtime),
              Type = predecessor.Node.AssociatedTitle.TitleType.Id,
              YearFrom = predecessor.Node.AssociatedTitle.ReleaseYear.Year,
              YearTo = predecessor.Node.AssociatedTitle.ReleaseYear.EndYear
            }
          );
        }

        previousEndCursor = predecessors?.PageInfo?.HasNextPage is true
          ? predecessors.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      if (preceding.Count is 0 && succeeding.Count is 0) {
        Collection = null;
        TriggerUpdateOnEnd(progressInfo);
        return;
      }

      Collection.Titles.AddRange(preceding);

      Collection.Titles.Add(
        new Title {
          Id = Id,
          Image = Posters.FirstOrDefault(),
          OriginalTitle = OriginalTitle,
          Rating = Rating,
          ReleaseDate = ReleaseDate,
          Runtime = Runtime,
          Type = Type,
          YearFrom = YearFrom,
          YearTo = YearTo
        }
      );

      Collection.Titles.AddRange(succeeding);

      int rated = 0;
      decimal ratingSum = 0;
      decimal ratingPercentageSum = 0;

      foreach (Title entry in Collection.Titles.Where(x => x.Rating.ValuePercentage is decimal)) {
        rated++;
        ratingSum += (decimal)entry.Rating.Value;
        ratingPercentageSum += (decimal)entry.Rating.ValuePercentage;
      }

      Collection.AverageRating = ratingSum / rated;
      Collection.AverageRatingPercentage = ratingPercentageSum / rated;

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchDirectorsAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchDirectors);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetCreditsAsync(
          category: "director",
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var credits = response.Data.Data.Title?.Credits;

        foreach (var credit in credits?.Edges?.EmptyIfNull()) {
          Crew.Directors.Add(
            new Person {
              Characters = credit.Node.Characters is null ? null : string.Join(" | ", credit.Node.Characters.Select(x => x.Name)),
              Id = credit.Node.Name.Id,
              ImageUrl = credit.Node.Name.PrimaryImage?.Url,
              Name = credit.Node.Name.NameText.Text,
              Url = $"https://www.imdb.com/de/name/{credit.Node.Name.Id}"
            }
          );
        }

        previousEndCursor = credits?.PageInfo?.HasNextPage is true
          ? credits.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchPlotAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchPlot);

      var response = await _client.TitleViaJson.GetPlotsAsync(
        maxNumberOfResults: 250,
        titleId: _id
      );

      progressInfo.Responses.Add(response);

      if (response is null || !response.IsSuccessful) {
        progressInfo.Error = true;
      }

      var plots = response.Data.Data.Title?.Plots;

      foreach (var plot in plots?.Edges?.EmptyIfNull()) {
        if (plot.Node.PlotType == "OUTLINE") {
          Plot.Outline = plot.Node.PlotText.PlainText;
        }

        if (plot.Node.PlotType == "SUMMARY") {
          Plot.Summaries.Add(plot.Node.PlotText.PlainText);
        }

        if (plot.Node.PlotType == "SYNOPSIS") {
          Plot.Synopsis = plot.Node.PlotText.PlainText;
        }
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchPostersAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchPosters);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetImagesAsync(
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id,
          type: "poster"
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var posters = response.Data.Data.Title?.Images;

        foreach (var poster in posters?.Edges?.EmptyIfNull()) {
          Posters.Add(
            new Image {
              Caption = poster.Node.Caption.PlainText,
              Height = poster.Node.Height,
              Id = poster.Node.Id,
              Url = poster.Node.Url,
              Width = poster.Node.Width
            }
          );
        }

        previousEndCursor = posters?.PageInfo?.HasNextPage is true
          ? posters.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchProductionCompaniesAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchProductionCompanies);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetCompanyCreditsAsync(
          category: "production",
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var companies = response.Data.Data.Title?.CompanyCredits;

        foreach (var company in companies?.Edges?.EmptyIfNull()) {
          ProductionCompanies.Add(
            new Company {
              Id = company.Node.Company.Id,
              Name = company.Node.Company.CompanyText.Text,
              Url = $"https://www.imdb.com/company/{company.Node.Company.Id}"
            }
          );
        }

        previousEndCursor = companies?.PageInfo?.HasNextPage is true
          ? companies.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchReleaseDatesAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchReleaseDates);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetReleaseDatesAsync(
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var releaseDates = response.Data.Data.Title?.ReleaseDates;

        foreach (var releaseDate in releaseDates?.Edges?.EmptyIfNull()) {
          ReleaseDates.Add(
            new ReleaseDate {
              CountryId = releaseDate.Node.Country.Id,
              Date = GetDateTime(releaseDate.Node),
              Note = releaseDate.Node.Attributes is null ? null : string.Join(" | ", releaseDate.Node.Attributes.Select(x => x.Text)),
            }
          );
        }

        previousEndCursor = releaseDates?.PageInfo?.HasNextPage is true
          ? releaseDates.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchSeasonsAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchSeasons);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetEpisodesAsync(
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var episodes = response.Data.Data.Title?.Episodes?.Episodes;

        if (episodes is null) {
          break;
        }

        foreach (var responseEpisode in episodes.Edges) {
          Season season = Seasons.FirstOrDefault(x => x.Id == responseEpisode.Node.Series.EpisodeNumber.SeasonNumber);

          if (season is null) {
            int seasonNumber = (int)responseEpisode.Node.Series.EpisodeNumber.SeasonNumber;

            season = new Season() {
              Id = seasonNumber,
              Url = $"https://www.imdb.com/title/{_id}/episodes/?season={seasonNumber}"
            };

            Seasons.Add(season);
          }

          Title episode = new Title {
            EpisodeNumber = responseEpisode.Node.Series.EpisodeNumber.EpisodeNumber,
            Id = responseEpisode.Node.Id,
            Image = GetImage(responseEpisode.Node.PrimaryImage),
            OriginalTitle = responseEpisode.Node.OriginalTitleText.Text,
            Rating = GetRating(responseEpisode.Node.RatingsSummary, responseEpisode.Node.Metacritic),
            ReleaseDate = GetDateTime(responseEpisode.Node.ReleaseDate),
            Runtime = GetRuntime(responseEpisode.Node.Runtime),
            SeasonNumber = responseEpisode.Node.Series.EpisodeNumber.SeasonNumber,
            Type = responseEpisode.Node.TitleType.Id,
            Url = $"https://www.imdb.com/title/{responseEpisode.Node.Id}",
            YearFrom = responseEpisode.Node.ReleaseYear.Year,
            YearTo = responseEpisode.Node.ReleaseYear.EndYear
          };

          season.Episodes.Add(episode);

          if (season.YearFrom is null || season.YearFrom > episode.YearFrom) {
            season.YearFrom = episode.YearFrom;
          }

          if (season.YearTo is null || season.YearTo < episode.YearFrom) {
            season.YearTo = episode.YearFrom;
          }

          if (season.YearTo < episode.YearTo) {
            season.YearTo = episode.YearTo;
          }
        }

        previousEndCursor = episodes.PageInfo.HasNextPage is true
          ? episodes.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      foreach (Season season in Seasons) {
        int rated = 0;
        decimal ratingSum = 0;
        decimal ratingPercentageSum = 0;

        foreach (Title episode in season.Episodes.Where(x => x.Rating.ValuePercentage is decimal)) {
          rated++;
          ratingSum += (decimal)episode.Rating.Value;
          ratingPercentageSum += (decimal)episode.Rating.ValuePercentage;
        }

        season.AverageRating = ratingSum / rated;
        season.AverageRatingPercentage = ratingPercentageSum / rated;
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchTitleAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchTitle);

      var response = await _client.TitleViaJson.GetTitleAsync(_id);

      progressInfo.Responses.Add(response);

      if (response is null || !response.IsSuccessful || response.Data.Data.Title is null) {
        progressInfo.Error = true;
      } else {
        var responseTitle = response.Data.Data.Title;

        Id = _id;
        IsAdult = responseTitle.IsAdult;
        OriginalTitle = responseTitle.OriginalTitleText.Text;
        Rating = GetRating(responseTitle.RatingsSummary, responseTitle.Metacritic);
        ReleaseDate = GetDateTime(responseTitle.ReleaseDate);
        Runtime = GetRuntime(responseTitle.Runtime);
        StillFrame = GetImage(responseTitle.StillFrame.Edges.FirstOrDefault()?.Node);
        Type = responseTitle.TitleType.Id;
        Url = $"https://www.imdb.com/title/{_id}";
        YearFrom = responseTitle.ReleaseYear.Year;
        YearTo = responseTitle.ReleaseYear.EndYear;

        if (!(responseTitle.PrestigiousAwardSummary is null)) {
          Awards.NumberOfPrestigiousNominations = responseTitle.PrestigiousAwardSummary.Nominations.Value;
          Awards.NumberOfPrestigiousWins = responseTitle.PrestigiousAwardSummary.Wins.Value;
          Awards.Prestigious = (
            responseTitle.PrestigiousAwardSummary.Award.WinningRank == 1
              ? "Won: "
              : "Nominated:"
          ) + responseTitle.PrestigiousAwardSummary.Award.Text;
        }

        if (!(responseTitle.CountriesOfOrigin is null)) {
          foreach (var country in responseTitle.CountriesOfOrigin.Countries) {
            Countries.Add(country.Id);
          }
        }

        if (!(responseTitle.Series is null)) {
          EpisodeNumber = responseTitle.Series.EpisodeNumber.EpisodeNumber;
          SeasonNumber = responseTitle.Series.EpisodeNumber.SeasonNumber;

          var series = responseTitle.Series.Series;
          if (!(series is null)) {
            Series = new Title {
              Id = series.Id,
              Image = GetImage(series.PrimaryImage),
              OriginalTitle = series.OriginalTitleText.Text,
              Rating = GetRating(series.RatingsSummary, series.Metacritic),
              ReleaseDate = GetDateTime(series.ReleaseDate),
              Runtime = GetRuntime(series.Runtime),
              Type = series.TitleType.Id,
              Url = $"https://www.imdb.com/title/{series.Id}",
              YearFrom = series.ReleaseYear.Year,
              YearTo = series.ReleaseYear.EndYear,
            };
          }
        }

        foreach (var genre in responseTitle.Genres.Genres) {
          Genres.Add(genre.Id);
        }

        foreach (var language in responseTitle.SpokenLanguages.SpokenLanguages) {
          Languages.Add(language.Id);
        }

        if (!(responseTitle.LatestTrailer is null)) {
          Trailer.Description = responseTitle.LatestTrailer.Description.Value;
          Trailer.Id = responseTitle.LatestTrailer.Id;
          Trailer.Name = responseTitle.LatestTrailer.Name.Value;
          Trailer.Runtime = TimeSpan.FromSeconds((int)responseTitle.LatestTrailer.Runtime.Value);
          Trailer.Thumbnail = responseTitle.LatestTrailer.Thumbnail.Url;
          Trailer.Url = $"https://www.imdb.com/video/{Trailer.Id}";
        }

        if (!(responseTitle.Plot is null)) {
          Plot.OutlineLocalized = responseTitle.Plot.PlotText.PlainText;
        }

        if (!(responseTitle.ProductionStatus is null)) {
          Status.Production = responseTitle.ProductionStatus.CurrentProductionStage.Id;
        }

        if (!(responseTitle.Meta is null)) {
          Status.Publication = responseTitle.Meta.PublicationStatus.ToLower();
        }
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private async Task FetchWritersAsync() {
      ProgressInfo progressInfo = TriggerUpdateOnBegin(WrapperMethod.FetchWriters);

      string previousEndCursor = string.Empty;

      while (!(previousEndCursor is null)) {
        var response = await _client.TitleViaJson.GetCreditsAsync(
          category: "writer",
          maxNumberOfResults: 250,
          previousEndCursor: previousEndCursor,
          titleId: _id
        );

        progressInfo.Responses.Add(response);

        if (response is null || !response.IsSuccessful) {
          progressInfo.Error = true;
          break;
        }

        var credits = response.Data.Data.Title?.Credits;

        foreach (var credit in credits?.Edges?.EmptyIfNull()) {
          Crew.Writers.Add(
            new Person {
              Characters = credit.Node.Characters is null ? null : string.Join(" | ", credit.Node.Characters.Select(x => x.Name)),
              Id = credit.Node.Name.Id,
              ImageUrl = credit.Node.Name.PrimaryImage?.Url,
              Name = credit.Node.Name.NameText.Text,
              Url = $"https://www.imdb.com/de/name/{credit.Node.Name.Id}"
            }
          );
        }

        previousEndCursor = credits?.PageInfo?.HasNextPage is true
          ? credits.PageInfo.EndCursor
          : null;

        OnUpdate?.Invoke(progressInfo);
      }

      TriggerUpdateOnEnd(progressInfo);
    }

    private DateTime? GetDateTime(RestApi.Responses.Json.ReleaseDate releaseDate) {
      if (releaseDate is null) {
        return null;
      }

      if (!(releaseDate.Day is int validDay)) {
        validDay = 1;
      }

      if (!(releaseDate.Month is int validMonth)) {
        validMonth = 1;
      }

      return new DateTime((int)releaseDate.Year, validMonth, validDay);
    }

    private Image GetImage(RestApi.Responses.Json.Image image) {
      if (image is null) {
        return null;
      }

      return new Image {
        Caption = image.Caption.PlainText,
        Height = image.Height,
        Id = image.Id,
        Url = image.Url,
        Width = image.Width
      };
    }

    private Rating GetRating(RestApi.Responses.Json.RatingsSummary ratingsSummary, RestApi.Responses.Json.Metacritic metacritic) {
      if (ratingsSummary is null) {
        return null;
      }

      Rating result = new Rating {
        TopRank = ratingsSummary.TopRanking?.Rank,
        Votes = ratingsSummary.VoteCount
      };

      if (!(ratingsSummary.AggregateRating is null)) {
        result.Value = (decimal)ratingsSummary.AggregateRating;
        result.ValuePercentage = (result.Value - 1) / 9 * 100;
      }

      if (!(metacritic is null)) {
        result.Metacritic = metacritic.Metascore.Score;
      }

      return result;
    }

    private TimeSpan? GetRuntime(RestApi.Responses.Json.Runtime runtime) {
      if (runtime is null) {
        return null;
      }

      return TimeSpan.FromSeconds((double)runtime.Seconds);
    }

    private ProgressInfo TriggerUpdateOnBegin(WrapperMethod method) {
      ProgressInfo progressInfo = new ProgressInfo() {
        Begin = DateTime.Now,
        Method = method,
      };

      OnUpdate?.Invoke(progressInfo);

      return progressInfo;
    }

    private void TriggerUpdateOnEnd(ProgressInfo progressInfo) {
      progressInfo.End = DateTime.Now;
      progressInfo.Duration = progressInfo.End - progressInfo.Begin;

      OnUpdate?.Invoke(progressInfo);
    }
  }
}
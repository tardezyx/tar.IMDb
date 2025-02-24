using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using tar.IMDb.Api.Enums;
using tar.IMDb.Api.Extensions;

namespace tar.IMDb.Api.RestApi.Requests {
  internal class Queryizer {
    private readonly Dictionary<NodeFragment, string> _nodeFragments;
    private readonly QueryOptions _options;
    private readonly Dictionary<QueryName, string> _queries;

    #region --- general fragments -----------------------------------------------------------------
    private string Attributes => $@"
      attributes {{
        text
      }}
    ";
    private string Award => $@"
      award {{
        category {{
          text
        }}
        event {{
          {IdAndText}
        }}
        eventEdition {{
          id
          instanceWithinYear
          year
        }}
        id
        notes {{
          {Body}
        }}
        text
        winningRank
      }}
    ";
    private string AwardedEntities => $@"
      awardedEntities {{
        ... on AwardedNames {{
          names {{
            id
            nameText {{
              text
            }}
            primaryImage {{
              {Image}
            }}
          }}
          secondaryAwardTitles {{
            note {{
              {Body}
            }}
            title {{
              id
              originalTitleText {{
                text
              }}
              series {{
                episodeNumber {{
                  episodeNumber
                  seasonNumber
                }}
              }}
            }}
          }}
        }}
        ... on AwardedTitles {{
          secondaryAwardNames {{
            {Name}
            note {{
              {Body}
            }}
          }}
          secondaryCompanies {{
            companyText {{
              text
            }}
          }}
          titles {{
            id
            originalTitleText {{
              text
            }}
            series {{
              episodeNumber {{
                episodeNumber
                seasonNumber
              }}
            }}
          }}
        }}
      }}
    ";
    private string Body => $@"
      markdown
      plaidHtml(showOriginalTitleText: {_options.ShowOriginalTitleText.ToString().ToLower()})
      plainText(showOriginalTitleText: {_options.ShowOriginalTitleText.ToString().ToLower()})
    ";
    private string Category => $@"
      category {{
        {IdAndText}
      }}
    ";
    private string Company => $@"
      company {{
        companyText {{
          text
        }}
        companyTypes {{
          {IdAndText}
        }}
        id
      }}
    ";
    private string Country => $@"
      country {{
        {IdAndText}
      }}
    ";
    private string DisplayableArticle => $@"
      displayableArticle {{
        body {{
          {Body}
        }}
      }}
    ";
    private string DisplayableProperty => $@"
      displayableProperty {{
        qualifiersInMarkdownList {{
          {Body}
        }}
        value {{
          {Body}
        }}
      }}
    ";
    private string IdAndText => $@"
      id
      text
    ";
    private string Image => $@"
      caption {{
        {Body}
      }}
      height
      id
      type
      url
      width
    ";
    private string InterestScore => $@"
      interestScore {{
        usersVoted
        usersInterested
      }}
    ";
    private string Language => $@"
      language {{
        {IdAndText}
      }}
    ";
    private string LifetimeGross => $@"
      total {{
        amount
        currency
      }}
    ";
    private string Name => $@"
      name {{
        id
        nameText {{
          text
        }}
        primaryImage {{
          {Image}
        }}
      }}
    ";
    private string OpeningWeekendGross => $@"
      gross {{
        total {{
          amount
          currency
        }}
      }}
      weekendEndDate
    ";
    private string TechnicalSpecifications => $@"
      technicalSpecifications {{
        aspectRatios {{
          items {{
            aspectRatio
            {Attributes}
          }}
        }}
        cameras {{
          items {{
            camera
            {Attributes}
          }}
        }}
        colorations {{
          items {{
            id
            conceptId
            text
            {Attributes}
          }}
        }}
        filmLengths {{
          items {{
            filmLength
            {Attributes}
          }}
        }}
        laboratories {{
          items {{
            laboratory
            {Attributes}
          }}
        }}
        negativeFormats {{
          items {{
            negativeFormat
            {Attributes}
          }}
        }}
        printedFormats {{
          items {{
            printedFormat
            {Attributes}
          }}
        }}
        processes {{
          items {{
            process
            {Attributes}
          }}
        }}
        soundMixes {{
          items {{
            id
            text
            {Attributes}
          }}
        }}
      }}
    ";
    private string Title => $@"
      {TitleBase}
      latestTrailer {{
        {NodeVideo}
      }}
      series {{
        episodeNumber {{
          episodeNumber
          seasonNumber
        }}
        nextEpisode {{
          id
          series {{
            episodeNumber {{
              episodeNumber
              seasonNumber
            }}
          }}
        }}
        previousEpisode {{
          id
          series {{
            episodeNumber {{
              episodeNumber
              seasonNumber
            }}
          }}
        }}
        series {{
          {TitleBase}
        }}
      }}
    ";
    private string TitleBase => $@"
      canRate {{
        isRatable
      }}
      certificate {{
        {NodeCertificate}
      }}
      countriesOfOrigin {{
        countries {{
          {IdAndText}
        }}
      }}
      episodes {{
        episodes(first: 0) {{
          total
        }}
        seasons: displayableSeasons(last: 1) {{
          total
        }}
      }}
      genres {{
        genres {{
          {IdAndText}
        }}
      }}
      id
      isAdult
      lifetimeGrossDomestic: lifetimeGross(boxOfficeArea: DOMESTIC) {{
        {LifetimeGross}
      }}
      lifetimeGrossInternational: lifetimeGross(boxOfficeArea: INTERNATIONAL) {{
        {LifetimeGross}
      }}
      lifetimeGrossWorldwide: lifetimeGross(boxOfficeArea: WORLDWIDE) {{
        {LifetimeGross}
      }}
      meta {{
        canonicalId
        publicationStatus
      }}
      metacritic {{
        metascore {{
          score
        }}
      }}
      openingWeekendGrossDomestic: openingWeekendGross(boxOfficeArea: DOMESTIC) {{
        {OpeningWeekendGross}
      }}
      openingWeekendGrossInternational: openingWeekendGross(boxOfficeArea: INTERNATIONAL) {{
        {OpeningWeekendGross}
      }}
      openingWeekendGrossWorldwide: openingWeekendGross(boxOfficeArea: WORLDWIDE) {{
        {OpeningWeekendGross}
      }}
      originalTitleText {{
        text
      }}
      plot {{
        author
        id
        plotText {{
          {Body}
        }}
        plotType
      }}
      prestigiousAwardSummary {{
        {Award}
        nominations
        wins
      }}
      primaryImage {{
        {Image}
      }}
      productionBudget {{
        budget {{
          amount
          currency
        }}
      }}
      productionStatus {{
        currentProductionStage {{
          {IdAndText}
        }}
        restriction {{
          restrictionReason
          unrestrictedTotal
        }}
      }}
      ratingsSummary {{
        aggregateRating
        notificationText {{
          value {{
            {Body}
          }}
        }}
        topRanking {{
          id
          rank
          text {{
            value
          }}
        }}
        voteCount
      }}
      releaseDate {{
        {Attributes}
        {Country}
        day
        month
        year
      }}
      releaseYear {{
        {Years}
      }}
      runtime {{
        {Attributes}
        id
        seconds
      }}
      spokenLanguages {{
        spokenLanguages {{
          {IdAndText}
        }}
      }}
      stillFrame: images(first: 1, filter: {{ types: [""still_frame""] }}) {{
        edges {{
          node {{
            {Image}
          }}
        }}
      }}
      {TechnicalSpecifications}
      {TitleText}
      {TitleType}
    ";
    private string TitleText => $@"
      titleText {{
        {Country}
        isOriginalTitle
        text
      }}
    ";
    private string TitleType => $@"
      titleType {{
        canHaveEpisodes
        id
        isSeries
        isEpisode
        text
      }}
    ";
    private string Years => $@"
      endYear
      year
    ";
    #endregion
    #region --- node fragments --------------------------------------------------------------------
    private string NodeAka => $@"
      {Attributes}
      {Country}
      {Language}
      text
    ";
    private string NodeAlexaTopQuestion => $@"
      attributeId
      answer {{
        {Body}
      }}
      question {{
        {Body}
      }}
    ";
    private string NodeAlternateVersion => $@"
      text {{
        {Body}
      }}
    ";
    private string NodeAwardNomination => $@"
      {Award}
      {AwardedEntities}
      id
      isWinner
    ";
    private string NodeCertificate => $@"
      {Attributes}
      {Country}
      id
      rating
      ratingReason
      ratingsBody {{
        {IdAndText}
      }}
    ";
    private string NodeCompanyCredit => $@"
      {Attributes}
      {Company}
      countries {{
        {IdAndText}
      }}
      {DisplayableProperty}
      yearsInvolved {{
        {Years}
      }}
    ";
    private string NodeConnection => $@"
      associatedTitle {{
        {TitleBase}
      }}
      {Category}
      description {{
        {Body}
      }}
      text
    ";
    private string NodeCrazyCredit => $@"
      id
      {InterestScore}
      text {{
        {Body}
      }}
    ";
    private string NodeCredit => $@"
      {Attributes}      
      {Category}
      {Name}
      ... on Cast {{
        characters(limit: 3) {{
          name
        }}
        episodeCredits(first: 0) {{
          total
          yearRange {{
            {Years}
          }}
        }}
      }}
      ... on Crew {{
        jobs {{
          {IdAndText}
        }}
      }}
    ";
    private string NodeEpisode => $@"
      {TitleBase}
      series {{
        episodeNumber {{
          episodeNumber
          seasonNumber
        }}
      }}
    ";
    private string NodeExternalLink => $@"
      externalLinkCategory {{
        {IdAndText}
      }}
      externalLinkLanguages {{
        {IdAndText}
      }}
      externalLinkRegion {{
        {IdAndText}
      }}
      label
      url
    ";
    private string NodeFaqEntry => $@"
      answer {{
        {Body}
      }}
      id
      isSpoiler
      question {{
        {Body}
      }}
    ";
    private string NodeFilmingDate => $@"
      endDate
      startDate
    ";
    private string NodeFilmingLocation => $@"
      {Attributes}
      id
      {InterestScore}
      location
      text
    ";
    private string NodeGoof => $@"
      {Category}
      id
      {InterestScore}
      isSpoiler
      text {{
        {Body}
      }}
    ";
    private string NodeImage => $@"
      {Image}
    ";
    private string NodeKeyword => $@"
      {InterestScore}
      keyword {{
        id
        text {{
          text
        }}
      }}
      legacyId
    ";
    private string NodeMoreLikeThisTitle => $@"
      {TitleBase}
    ";
    private string NodeNewsEntry => $@"
      articleTitle {{
        {Body}
      }}
      byline
      date
      externalUrl
      id
      image {{
        {Image}
      }}
      source {{
        homepage {{
          label
          url
        }}
        trustedSource
      }}
      text {{
        {Body}
      }}
    ";
    private string NodeParentsGuideEntry => $@"
      {Category}
      id
      isSpoiler
      text {{
        {Body}
      }}
    ";
    private string NodePlot => $@"
      author
      id
      plotText {{
        {Body}
      }}
      plotType
    ";
    private string NodeProductionDate => $@"
      endDate {{
        date
        displayableProperty {{
          value {{
            {Body}
          }}
        }}
      }}
      startDate {{
        date
        displayableProperty {{
          value {{
            {Body}
          }}
        }}
      }}
    ";
    private string NodeQuote => $@"
      {DisplayableArticle}
      id
      {InterestScore}
      isSpoiler
    ";
    private string NodeReleaseDate => $@"
      {Attributes}
      {Country}
      day
      month
      year
    ";
    private string NodeReview => $@"
      author {{
        nickName
        userId
      }}
      authorRating
      helpfulness {{
        downVotes
        upVotes
      }}
      id
      spoiler
      submissionDate
      summary {{
        originalText
      }}
      text {{
        originalText {{
          {Body}
        }}
      }}
    ";
    private string NodeRuntime => $@"
      {Attributes}
      id
      seconds
    ";
    private string Nodes(NodeFragment nodeFragment) => $@"
      edges {{
        cursor
        node {{
          {_nodeFragments[nodeFragment]}
        }}
        position
      }}
      pageInfo {{
        endCursor
        hasNextPage
        hasPreviousPage
        startCursor
      }}
      total
    ";
    private string NodeSoundtrackEntry => $@"
      id
      comments {{
        {Body}
      }}
      text
    ";
    private string NodeTagline => $@"
      text
    ";
    private string NodeTriviaEntry => $@"
      {Category}
      id
      {InterestScore}
      isSpoiler
      text {{
        {Body}
      }}
    ";
    private string NodeVideo => $@"
      contentType {{
        displayName {{
          value
        }}
        id
      }}
      description {{
        value
      }}
      id
      name {{
        value
      }}
      primaryTitle {{
        {TitleBase}
        series {{
          episodeNumber {{
            episodeNumber
            seasonNumber
          }}
        }}
      }}
      providerType {{
        id
      }}
      runtime {{
        unit
        value
      }}
      thumbnail {{
        height
        width
        url
      }}
    ";
    #endregion
    #region --- queries ---------------------------------------------------------------------------
    private string QueryTitle => $@"
      query {nameof(QueryTitle).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          {Title}
        }}
      }}
    ";
    private string QueryTitleAggregateRatingsBreakdown => $@"
      query {nameof(QueryTitleAggregateRatingsBreakdown).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          aggregateRatingsBreakdown {{
            {(
                _options.Country is null
                  ? "histogram {"
                  : $"histogram(demographicFilter: {{country: {_options.Country}}}) {{"
              )}
              histogramValues {{
                rating
                voteCount
              }}
            }}
            ratingsSummaryByCountry {{
              aggregate
              country
              displayText {{
                value
              }}
              voteCount
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleAkas => $@"
      query {nameof(QueryTitleAkas).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          akas(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
            sort: {{ by: RELEVANCE, order: ASC }}
          ) {{
            {Nodes(NodeFragment.Aka)}
          }}
        }}
      }}
    ";
    private string QueryTitleAlexaTopQuestions => $@"
      query {nameof(QueryTitleAlexaTopQuestions).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          alexaTopQuestions(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            edges {{
              node {{
                {NodeAlexaTopQuestion}
              }}
            }}
            pageInfo {{
              hasNextPage
              hasPreviousPage
              endCursor
              startCursor
            }}
            total
          }}
        }}
      }}
    ";
    private string QueryTitleAlternateVersions => $@"
      query {nameof(QueryTitleAlternateVersions).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          alternateVersions(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.AlternateVersion)}
          }}
        }}
      }}
    ";
    private string QueryTitleAwardNominations => $@"
      query {nameof(QueryTitleAwardNominations).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          awardNominations(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.AwardsFilter == AwardsFilter.NominationsOnly
                  ? "filter: { wins: EXCLUDE_WINS }"
                  : _options.AwardsFilter == AwardsFilter.WinsOnly
                    ? "filter: { wins: WINS_ONLY }"
                    : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
            sort: {{ by: PRESTIGIOUS, order: DESC }}
          ) {{
            {Nodes(NodeFragment.AwardNomination)}
          }}
        }}
      }}
    ";
    private string QueryTitleCertificates => $@"
      query {nameof(QueryTitleCertificates).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          certificates(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Certificate)}
          }}
        }}
      }}
    ";
    private string QueryTitleCompanyCreditCategories => $@"
      query {nameof(QueryTitleCompanyCreditCategories).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          companyCreditCategories {{
            {Category}
            companyCredits(first: {_options.MaxNumberOfResults}) {{
              {Nodes(NodeFragment.CompanyCredit)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleCompanyCredits => $@"
      query {nameof(QueryTitleCompanyCredits).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          companyCredits(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.Category.HasText()
                  ? $@"filter: {{categories: ""{_options.Category}""}}"
                  : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.CompanyCredit)}
          }}
        }}
      }}
    ";
    private string QueryTitleConnectionCategories => $@"
      query {nameof(QueryTitleConnectionCategories).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          connectionCategories {{
            {Category}
            connections(first: {_options.MaxNumberOfResults}) {{
              {Nodes(NodeFragment.Connection)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleConnections => $@"
      query {nameof(QueryTitleConnections).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          connections(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.Category.HasText()
                  ? $@"filter: {{categories: ""{_options.Category}""}}"
                  : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Connection)}
          }}
        }}
      }}
    ";
    private string QueryTitleCrazyCredits => $@"
      query {nameof(QueryTitleCrazyCredits).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          crazyCredits(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.CrazyCredit)}
          }}
        }}
      }}
    ";
    private string QueryTitleCreditCategories => $@"
      query {nameof(QueryTitleCreditCategories).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          creditCategories {{
            {Category}
            credits(first: {_options.MaxNumberOfResults}) {{
              {Nodes(NodeFragment.Credit)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleCredits => $@"
      query {nameof(QueryTitleCredits).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          credits(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.Category.HasText()
                  ? $@"filter: {{categories: ""{_options.Category}""}}"
                  : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Credit)}
          }}
        }}
      }}
    ";
    private string QueryTitleEpisodes => $@"
      query {nameof(QueryTitleEpisodes).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          episodes {{
            episodes(
              after: ""{_options.PreviousEndCursor}""
              {(
                  _options.Season is null
                    ? string.Empty
                    : $@"filter: {{includeSeasons: [""{_options.Season}""]}}"
                )}
              first: {_options.MaxNumberOfResults}
              sort: {{ by: EPISODE_THEN_RELEASE, order: ASC }}
            ) {{
              {Nodes(NodeFragment.Episode)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleExternalLinkCategories => $@"
      query {nameof(QueryTitleExternalLinkCategories).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          externalLinkCategories {{
            {Category}
            externalLinks(first: {_options.MaxNumberOfResults}) {{
              {Nodes(NodeFragment.ExternalLink)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleExternalLinks => $@"
      query {nameof(QueryTitleExternalLinks).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          externalLinks(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.Category.HasText()
                  ? $@"filter: {{categories: ""{_options.Category}""}}"
                  : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.ExternalLink)}
          }}
        }}
      }}
    ";
    private string QueryTitleFaqs => $@"
      query {nameof(QueryTitleFaqs).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          faqs(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.FaqEntry)}
          }}
        }}
      }}
    ";
    private string QueryTitleFilmingDates => $@"
      query {nameof(QueryTitleFilmingDates).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          filmingDates(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.FilmingDate)}
          }}
        }}
      }}
    ";
    private string QueryTitleFilmingLocations => $@"
      query {nameof(QueryTitleFilmingLocations).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          filmingLocations(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.FilmingLocation)}
          }}
        }}
      }}
    ";
    private string QueryTitleGoofCategories => $@"
      query {nameof(QueryTitleGoofCategories).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          goofCategories {{
            {Category}
            goofs(first: {_options.MaxNumberOfResults}) {{
              {Nodes(NodeFragment.Goof)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleGoofs => $@"
      query {nameof(QueryTitleGoofs).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          goofs(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.Category.HasText()
                  ? $@"filter: {{categories: ""{_options.Category}""}}"
                  : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Goof)}
          }}
        }}
      }}
    ";
    private string QueryTitleImages => $@"
      query {nameof(QueryTitleImages).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          images(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.Type.HasText()
                  ? $@"filter: {{types: ""{_options.Type}""}}"
                  : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Image)}
          }}
        }}
      }}
    ";
    private string QueryTitleImageTypes => $@"
      query {nameof(QueryTitleImageTypes).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          imageTypes {{
            images(first: {_options.MaxNumberOfResults}) {{
              {Nodes(NodeFragment.Image)}
            }}
            imageType {{
              imageTypeId
              text
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleKeywords => $@"
      query {nameof(QueryTitleKeywords).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          keywords(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Keyword)}
          }}
        }}
      }}
    ";
    private string QueryTitleMoreLikeThisTitles => $@"
      query {nameof(QueryTitleMoreLikeThisTitles).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          moreLikeThisTitles(first: {_options.MaxNumberOfResults}) {{
            edges {{
              cursor
              node {{
                {TitleBase}
              }}
              position
            }}
            pageInfo {{
              endCursor
              hasNextPage
              hasPreviousPage
              startCursor
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleNews => $@"
      query {nameof(QueryTitleNews).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          news(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            total
            pageInfo {{
              hasNextPage
              endCursor
            }}
            edges {{
              node {{
                {NodeNewsEntry}
              }}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleParentsGuide => $@"
      query {nameof(QueryTitleParentsGuide).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          parentsGuide {{
            guideItems(
              after: ""{_options.PreviousEndCursor}""
              {(
                  _options.Category.HasText()
                    ? $@"filter: {{categories: ""{_options.Category}""}}"
                    : string.Empty
                )}
              first: {_options.MaxNumberOfResults}
            ) {{
              {Nodes(NodeFragment.ParentsGuideEntry)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleParentsGuideCategories => $@"
      query {nameof(QueryTitleParentsGuideCategories).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          parentsGuideCategories: parentsGuide {{
            categories {{
              {Category}
              guideItems(first: {_options.MaxNumberOfResults}) {{
                {Nodes(NodeFragment.ParentsGuideEntry)}
              }}
              severity {{
                  id
                  text
                  votedFor
                  voteType
              }}
              severityBreakdown {{
                id
                text
                votedFor
                voteType
              }}
              totalSeverityVotes
            }}
          }}
        }}
      }}
    ";
    private string QueryTitlePlots => $@"
      query {nameof(QueryTitlePlots).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          plots(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.PlotType == PlotType.Outline
                  ? $@"filter: {{type: OUTLINE}}"
                  : _options.PlotType == PlotType.Summary
                    ? $@"filter: {{type: SUMMARY}}"
                    : _options.PlotType == PlotType.Synopsis
                      ? $@"filter: {{type: SYNOPSIS}}"
                      : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Plot)}
          }}
        }}
      }}
    ";
    private string QueryTitleProductionDates => $@"
      query {nameof(QueryTitleProductionDates).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          productionDates(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.ProductionDate)}
          }}
        }}
      }}
    ";
    private string QueryTitleQuotes => $@"
      query {nameof(QueryTitleQuotes).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          quotes(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Quote)}
          }}
        }}
      }}
    ";
    private string QueryTitleReleaseDates => $@"
      query {nameof(QueryTitleReleaseDates).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          releaseDates(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.ReleaseDate)}
          }}
        }}
      }}
    ";
    private string QueryTitleReviews => $@"
      query {nameof(QueryTitleReviews).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          reviews(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            edges {{
              cursor
              node {{
                {NodeReview}
              }}
            }}
            pageInfo {{
              hasNextPage
              hasPreviousPage
              endCursor
              startCursor
            }}
            total
          }}
        }}
      }}
    ";
    private string QueryTitleRuntimes => $@"
      query {nameof(QueryTitleRuntimes).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          runtimes(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Runtime)}
          }}
        }}
      }}
    ";
    private string QueryTitleSoundtrack => $@"
      query {nameof(QueryTitleSoundtrack).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          soundtrack(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.SoundtrackEntry)}
          }}
        }}
      }}
    ";
    private string QueryTitleTaglines => $@"
      query {nameof(QueryTitleTaglines).GetSubstringAfterString("Query")} {{
        title(id: ""{_options.TitleId}"") {{
          taglines(
            after: ""{_options.PreviousEndCursor}""
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.Tagline)}
          }}
        }}
      }}
    ";
    private string QueryTitleTrivia => $@"
      query TitleTrivia {{
        title(id: ""{_options.TitleId}"") {{
          trivia(
            after: ""{_options.PreviousEndCursor}""
            {(
                _options.Category.HasText()
                  ? $@"filter: {{categories: ""{_options.Category}""}}"
                  : string.Empty
              )}
            first: {_options.MaxNumberOfResults}
          ) {{
            {Nodes(NodeFragment.TriviaEntry)}
          }}
        }}
      }}
    ";
    private string QueryTitleTriviaCategories => $@"
      query TitleTriviaCategories {{
        title(id: ""{_options.TitleId}"") {{
          triviaCategories {{
            {Category}
            trivia(first: {_options.MaxNumberOfResults}) {{
              {Nodes(NodeFragment.TriviaEntry)}
            }}
          }}
        }}
      }}
    ";
    private string QueryTitleVideos => $@"
      query TitleVideos {{
        title(id: ""{_options.TitleId}"") {{
          videoStrip(
            after: ""{_options.PreviousEndCursor}""
            filter: {{maturityLevel: INCLUDE_MATURE, types: TRAILER}}
            first: {_options.MaxNumberOfResults}
          ) {{
            facets {{
              names {{
                name {{
                  id
                  nameText {{
                    text
                  }}
                }}
                total
              }}
              types {{
                total
                type {{
                  displayName {{
                    value
                  }}
                  id
                }}
              }}
            }}
            {Nodes(NodeFragment.Video)}
          }}
        }}
      }}
    ";
    #endregion

    internal Queryizer(QueryOptions options) {
      _options = options;
      _nodeFragments = InitNodeFragments();
      _queries = InitQueries();
    }

    private string GetCleanQuery(QueryName queryName) {
      string query = _queries[queryName];

      if (query.IsNullOrEmpty()) {
        return string.Empty;
      }

      int spaces = 0;
      StringBuilder stringBuilder = new StringBuilder();

      foreach (string line in Regex.Split(query, "\r\n|\r|\n")) {
        string cleanLine = line.Trim();

        if (cleanLine.IsNullOrEmpty()) {
          continue;
        }

        int openBrackets1 = cleanLine.GetOccurrences('(');
        int openBrackets2 = cleanLine.GetOccurrences('{');
        int closeBrackets1 = cleanLine.GetOccurrences(')');
        int closeBrackets2 = cleanLine.GetOccurrences('}');

        if (openBrackets1 < closeBrackets1 || openBrackets2 < closeBrackets2) {
          spaces--;
        }

        for (int i = 0; i < spaces; i++) {
          stringBuilder.Append("  ");
        }

        stringBuilder.Append(cleanLine + Environment.NewLine);

        if (openBrackets1 > closeBrackets1 || openBrackets2 > closeBrackets2) {
          spaces++;
        }
      }

      return stringBuilder.ToString();
    }

    internal RequestBody GetRequestBody(QueryName queryName) {
      return new RequestBody() {
        OperationName = queryName.ToString(),
        Query = GetCleanQuery(queryName),
        Variables = new RequestBodyVariables() {
          Locale = _options.Language
        }
      };
    }

    private Dictionary<NodeFragment, string> InitNodeFragments() {
      return new Dictionary<NodeFragment, string>() {
        {NodeFragment.Aka, NodeAka},
        {NodeFragment.AlexaTopQuestion, NodeAlexaTopQuestion},
        {NodeFragment.AlternateVersion, NodeAlternateVersion},
        {NodeFragment.AwardNomination, NodeAwardNomination},
        {NodeFragment.Certificate, NodeCertificate},
        {NodeFragment.CompanyCredit, NodeCompanyCredit},
        {NodeFragment.Connection, NodeConnection},
        {NodeFragment.CrazyCredit, NodeCrazyCredit},
        {NodeFragment.Credit, NodeCredit},
        {NodeFragment.Episode, NodeEpisode},
        {NodeFragment.ExternalLink, NodeExternalLink},
        {NodeFragment.FaqEntry, NodeFaqEntry},
        {NodeFragment.FilmingDate, NodeFilmingDate},
        {NodeFragment.FilmingLocation, NodeFilmingLocation},
        {NodeFragment.Goof, NodeGoof},
        {NodeFragment.Image, NodeImage},
        {NodeFragment.Keyword, NodeKeyword},
        {NodeFragment.MoreLikeThisTitle, NodeMoreLikeThisTitle},
        {NodeFragment.NewsEntry, NodeNewsEntry},
        {NodeFragment.ParentsGuideEntry, NodeParentsGuideEntry},
        {NodeFragment.Plot, NodePlot},
        {NodeFragment.ProductionDate, NodeProductionDate},
        {NodeFragment.Quote, NodeQuote},
        {NodeFragment.ReleaseDate, NodeReleaseDate},
        {NodeFragment.Review, NodeReview},
        {NodeFragment.Runtime, NodeRuntime},
        {NodeFragment.SoundtrackEntry, NodeSoundtrackEntry},
        {NodeFragment.Tagline, NodeTagline},
        {NodeFragment.TriviaEntry, NodeTriviaEntry},
        {NodeFragment.Video, NodeVideo}
      };
    }

    private Dictionary<QueryName, string> InitQueries() {
      return new Dictionary<QueryName, string>() {
        {QueryName.Title, QueryTitle},
        {QueryName.TitleAggregateRatingsBreakdown, QueryTitleAggregateRatingsBreakdown},
        {QueryName.TitleAkas, QueryTitleAkas},
        {QueryName.TitleAlexaTopQuestions, QueryTitleAlexaTopQuestions},
        {QueryName.TitleAlternateVersions, QueryTitleAlternateVersions},
        {QueryName.TitleAwardNominations, QueryTitleAwardNominations},
        {QueryName.TitleCertificates, QueryTitleCertificates},
        {QueryName.TitleCompanyCreditCategories, QueryTitleCompanyCreditCategories},
        {QueryName.TitleCompanyCredits, QueryTitleCompanyCredits},
        {QueryName.TitleConnectionCategories, QueryTitleConnectionCategories},
        {QueryName.TitleConnections, QueryTitleConnections},
        {QueryName.TitleCrazyCredits, QueryTitleCrazyCredits},
        {QueryName.TitleCreditCategories, QueryTitleCreditCategories},
        {QueryName.TitleCredits, QueryTitleCredits},
        {QueryName.TitleEpisodes, QueryTitleEpisodes},
        {QueryName.TitleExternalLinkCategories, QueryTitleExternalLinkCategories},
        {QueryName.TitleExternalLinks, QueryTitleExternalLinks},
        {QueryName.TitleFaqs, QueryTitleFaqs},
        {QueryName.TitleFilmingDates, QueryTitleFilmingDates},
        {QueryName.TitleFilmingLocations, QueryTitleFilmingLocations},
        {QueryName.TitleGoofCategories, QueryTitleGoofCategories},
        {QueryName.TitleGoofs, QueryTitleGoofs},
        {QueryName.TitleImages, QueryTitleImages},
        {QueryName.TitleImageTypes, QueryTitleImageTypes},
        {QueryName.TitleKeywords, QueryTitleKeywords},
        {QueryName.TitleMoreLikeThisTitles, QueryTitleMoreLikeThisTitles},
        {QueryName.TitleNews, QueryTitleNews},
        {QueryName.TitleParentsGuide, QueryTitleParentsGuide},
        {QueryName.TitleParentsGuideCategories, QueryTitleParentsGuideCategories},
        {QueryName.TitlePlots, QueryTitlePlots},
        {QueryName.TitleProductionDates, QueryTitleProductionDates},
        {QueryName.TitleQuotes, QueryTitleQuotes},
        {QueryName.TitleReleaseDates, QueryTitleReleaseDates},
        {QueryName.TitleReviews, QueryTitleReviews},
        {QueryName.TitleRuntimes, QueryTitleRuntimes},
        {QueryName.TitleSoundtrack, QueryTitleSoundtrack},
        {QueryName.TitleTaglines, QueryTitleTaglines},
        {QueryName.TitleTrivia, QueryTitleTrivia},
        {QueryName.TitleTriviaCategories, QueryTitleTriviaCategories},
        {QueryName.TitleVideos, QueryTitleVideos}
      };
    }
  }
}
using System.Diagnostics;
using tar.IMDb.Api;
using tar.IMDb.Api.Wrapper;

namespace tar.IMDb.Examples {
  public class TestWrapper {
    public TimeSpan Duration { get; private set; }
    public IMDbTitle IMDbTitle { get; private set; }
    public List<ProgressInfo> Progress { get; private set; } = [];

    public TestWrapper(string titleId) {
      IMDbTitle = new IMDbTitle(titleId);
      IMDbTitle.OnUpdate += OnUpdate;
    }

    public async Task RunTestAsync() {
      Stopwatch stopwatch = Stopwatch.StartNew();
      await IMDbTitle.FetchAsync();
      stopwatch.Stop();
      Duration = stopwatch.Elapsed;
    }

    private void OnUpdate(ProgressInfo progressInfo) {
      if (Progress.FirstOrDefault(x => x.Method == progressInfo.Method) is ProgressInfo existingProgressInfo) {
        existingProgressInfo = progressInfo;
      } else {
        Progress.Add(progressInfo);
      }
    }
  }
}
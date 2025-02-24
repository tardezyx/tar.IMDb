using System.Collections.Generic;
using System.Linq;

namespace tar.IMDb.Api.Extensions {
  public static partial class Extensions {
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) {
      return source ?? Enumerable.Empty<T>();
    }
  }
}
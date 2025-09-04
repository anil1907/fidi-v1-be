using VsaSample.SharedKernel.Results;
using Xunit;

namespace VsaSample.Application.UnitTests.Results;

public class PagedResultTests
{
    [Fact]
    public void Create_ShouldSetPagingInformationCorrectly()
    {
        var items = new[] { 1, 2, 3, 4, 5 };
        var result = PagedResult<int>.Create(items, pageNumber: 1, pageSize: 5, totalCount: 10);

        Assert.True(result.HasNextPage);
        Assert.False(result.HasPreviousPage);
        Assert.Equal(10, result.TotalCount);
        Assert.Equal(5, result.PageSize);
    }
}

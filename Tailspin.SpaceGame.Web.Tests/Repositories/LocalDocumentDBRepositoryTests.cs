using Moq;
using TailSpin.SpaceGame.Web;
using TailSpin.SpaceGame.Web.Models;

namespace Tailspin.SpaceGame.Web.Tests.Repositories
{
    public class LocalDocumentDBRepositoryTests
    {
        private IDocumentDBRepository<Score> _scoreRepository;

        private Mock<IDocumentDBRepository<Score>> _scoreRepositoryMock = new Mock<IDocumentDBRepository<Score>>();

        [SetUp]
        public void Setup()
        {
            _scoreRepository = _scoreRepositoryMock.Object;
        }

        //[Test]
        //public void Test1()
        //{
        //    Assert.Pass();
        //}

        [TestCase("Milky Way")]
        [TestCase("Andromeda")]
        [TestCase("Pinwheel")]
        [TestCase("NGC 1300")]
        [TestCase("Messier 82")]
        public void FetchOnlyRequestedGameRegion(string gameRegion)
        {
            const int PAGE = 0; // take the first page of results
            const int MAX_RESULTS = 10; // sample up to 10 results

            // Form the query predicate.
            // This expression selects all scores for the provided game region.
            //Expression<Func<Score, bool>> queryPredicate = score => (score.GameRegion == gameRegion);
            Func<Score, bool> queryPredicate = score => score.GameRegion == gameRegion;

            // Fetch the scores.
            Task<IEnumerable<Score>> scoresTask = _scoreRepository.GetItemsAsync(
                queryPredicate: queryPredicate, // the predicate defined above
                orderDescendingPredicate: score => 1, // we don't care about the order
                page: PAGE,
                pageSize: MAX_RESULTS
            );

            IEnumerable<Score> scores = scoresTask.Result;

            // Verify that each score's game region matches the provided game region.
            Assert.That(scores, Is.All.Matches<Score>(score => score.GameRegion == gameRegion));
        }
    }
}
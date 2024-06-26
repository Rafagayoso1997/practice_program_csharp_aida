using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace ReadBooks.Tests
{
    public class ReadBooksServiceTest
    {
        private FriendsRepository _friendsRepository;
        private BooksRepository _booksRepository;
        private Session _session;
        private ReadBooksService _readBooksService;
        private User _requestUser;
        private User _loggedUser;
        private Guid _loggedUserId;

        [SetUp]
        public void Setup()
        {
            _friendsRepository = Substitute.For<FriendsRepository>();
            _booksRepository = Substitute.For<BooksRepository>();
            _session = Substitute.For<Session>();
            _readBooksService = new(_friendsRepository, _session, _booksRepository);
            _loggedUserId = Guid.NewGuid();
           _loggedUser = new User(_loggedUserId);
           _requestUser = new(Guid.NewGuid());
        }

        [Test]
        public void user_is_not_logged_throw_exception()
        {
            _session.GetLoggedUser().ReturnsNull();

            Action act = () => _readBooksService.GetBooksReadByUser(_requestUser);

            act.Should()
                .Throw<UserNotLoggedException>()
                .WithMessage("The user is not logged");
        }

        [Test]
        public void given_user_and_logged_user_are_not_friends()
        {
            _session.GetLoggedUser().Returns(_loggedUser);
            _friendsRepository
                .GetFriendsOf(_requestUser.Id)
                .Returns(Enumerable.Empty<User>());

            var booksReadByUser = _readBooksService.GetBooksReadByUser(_requestUser);

            booksReadByUser.Should().BeEmpty();
        }

        [Test]
        public void given_user_and_logged_user_are_friends_then_return_read_books_from_given_user()
        {
            var anyTitle = "Pragmatic programmer";
            _session.GetLoggedUser().Returns(_loggedUser);
            _friendsRepository
                .GetFriendsOf(_requestUser.Id)
                .Returns(new List<User> { new User(_loggedUserId) });
            _booksRepository
                .GetBooksReadBy(_requestUser.Id)
                .Returns(new List<Book> { new Book(anyTitle) });

            var booksReadByUser = _readBooksService.GetBooksReadByUser(_requestUser);

            booksReadByUser.Should().BeEquivalentTo(new List<Book>
            {
                new Book(anyTitle)
            });
        }
    }
}
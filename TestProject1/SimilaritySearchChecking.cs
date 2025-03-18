using Moq;

namespace TestProject1;

public class SimilaritySearchChecking
{
    private readonly Mock<IBookRepository> _bookRepo;
    private readonly BookService _bookService;

    public SimilaritySearchChecking()
    {
        _bookRepo = new Mock<IBookRepository>();
        _bookService = new BookService(_bookRepo.Object);
    }
    
}
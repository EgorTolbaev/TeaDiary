public class TestDataFixture : IDisposable
{
    public Guid TeaTypeId { get; private set; }

    public TestDataFixture()
    {
        TeaTypeId = Guid.NewGuid();
    }

    public void Dispose()
    {
        // Очистка при необходимости
    }
}
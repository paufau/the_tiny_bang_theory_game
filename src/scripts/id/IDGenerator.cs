
namespace Utils.ID
{
    public class IDGenerator
    {
        private long id;

        public long Next()
        {
            id++;
            return id;
        }
    }
}


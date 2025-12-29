namespace HelpResponse.AI.Domain.Challenges
{
    public class Challenge(int id)
    {
        public int Id { get; set; } = id;


        public bool IsValid()
        {
            if (Id <= 0)
                return false;

            return true;
        }
    }
}
using AssignmentDay4.Model;

namespace AssignmentDay4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserData user1 = new UserData("vaish");
            UserData.DisplayDetails();
            UserData user2 = new UserData("Navi");
            UserData.DisplayDetails(); 
            UserData user3 = new UserData("Bhambure");
            UserData.DisplayDetails(); 
            UserData user4 = new UserData("Harsh");
            UserData.DisplayDetails();
            UserData user5 = new UserData("Sakshi");
            UserData.DisplayDetails(); 
            UserData user6 = new UserData("Rahul");
            UserData.DisplayDetails();
           


        }
    }
}

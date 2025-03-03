
using System.Collections;


namespace AssignmentDay6
{
    class WorkshopRegistration
    {
        private Hashtable workshopRegistrations = new Hashtable();

        public WorkshopRegistration()
        {
            workshopRegistrations["Robotics"] = new HashSet<int>();
            workshopRegistrations["Gen AI"] = new HashSet<int>();
            workshopRegistrations["ML"] = new HashSet<int>();
            workshopRegistrations["Cyber Security"] = new HashSet<int>();
        }
        // Register a student for a workshop
        public void RegisterStudent(string workshop, int studentID)
        {
            if (!workshopRegistrations.ContainsKey(workshop))
            {
                Console.WriteLine($"Invalid workshop name: {workshop}. Available workshops: Robotics, Gen AI, ML, Cyber Security.");
            return;
            }

            HashSet<int> registeredStudents = (HashSet<int>)workshopRegistrations[workshop];

            if (registeredStudents.Contains(studentID))
            {
                Console.WriteLine($"Student {studentID} is already registered for {workshop}.");
            }
            else
            {
                registeredStudents.Add(studentID);
                Console.WriteLine($"Student {studentID} successfully registered for {workshop}.");
            }
        }

        // Display all registered students for a workshop
        public void DisplayRegistrations(string workshop)
        {
            if (workshopRegistrations.ContainsKey(workshop))
            {
                HashSet<int> registeredStudents = (HashSet<int>)workshopRegistrations[workshop];
                if (registeredStudents.Count > 0)
                {
                    Console.WriteLine($"Students registered for {workshop}: {string.Join(", ", registeredStudents)}");
                }
                else
                {
                    Console.WriteLine($"No students registered for {workshop}.");
                }
            }
            else
            {
                Console.WriteLine($"No registrations found for {workshop}.");
            }

        }
    }
}

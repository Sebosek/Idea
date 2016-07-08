using Idea.Tests.Entity;

namespace Idea.Tests.Fixture.Seed
{
    public class BookSeed
    {
        public static int ID_HARRY_POTTER_I = 3000;
        public static int ID_HARRY_POTTER_II = 3001;
        public static int ID_HARRY_POTTER_III = 3002;
        //public static int ID_HARRY_POTTER_IV = 3003;
        //public static int ID_HARRY_POTTER_V = 3004;
        //public static int ID_HARRY_POTTER_VI = 3005;
        //public static int ID_HARRY_POTTER_VII = 3006;
        public static int ID_KAFKA_ON_THE_STORE = 3010;
        public static int ID_LIBRARY = 3011;
        public static int ID_LORD_OF_THE_RINGS_I = 3020;
        public static int ID_LORD_OF_THE_RINGS_II = 3021;
        public static int ID_LORD_OF_THE_RINGS_III = 3022;
        public static int ID_ALCHEMIST = 3030;
        public static int ID_ELEVEN_MINUTES = 3031;
        public static int ID_HAUNTED = 3040;
        public static int ID_FIGHT_CLUB = 3041;

        public static Book HARRY_POTTER_I = new Book
        {
            Title = "Harry Potter and the Philosopher's Stone",
        };

        public static Book HARRY_POTTER_II = new Book
        {
            Title = "Harry Potter and the Chambre of Secrets"
        };

        public static Book HARRY_POTTER_III = new Book
        {
            Title = "Harry Potter and the Prisoner of Azkaban"
        };

        public static Book KAFKA_ON_THE_STORE = new Book
        {
            Title = "Kafka on the store"
        };

        public static Book LIBRARY = new Book
        {
            Title = "The strange library"
        };

        public static Book LORD_OF_THE_RINGS_I = new Book
        {
            Title = "Lord of the rings - The Fellowship of the Ring"
        };

        public static Book LORD_OF_THE_RINGS_II = new Book
        {
            Title = "Lord of the rings - The Two Towers"
        };

        public static Book LORD_OF_THE_RINGS_III = new Book
        {
            Title = "Lord of the rings - The Return of the King"
        };

        public static Book ALCHEMIST = new Book
        {
            Title = "Alchemist"
        };

        public static Book ELEVEN_MINUTES = new Book
        {
            Title = "Eleven minutes"
        };

        public static Book HAUNTED = new Book
        {
            Title = "Haunted"
        };

        public static Book FIGHT_CLUB = new Book
        {
            Title = "Fight club"
        };

        public static Book[] BOOKS_DATA = new Book[]
        {
              ALCHEMIST
            , ELEVEN_MINUTES
            , FIGHT_CLUB
            , HARRY_POTTER_I
            , HARRY_POTTER_II
            , HARRY_POTTER_III
            , HAUNTED
            , KAFKA_ON_THE_STORE
            , LIBRARY
            , LORD_OF_THE_RINGS_I
            , LORD_OF_THE_RINGS_II
            , LORD_OF_THE_RINGS_III
        };
    }
}

﻿using Idea.Tests.Entity;

namespace Idea.Tests.Fixture.Seed
{
    public class AuthorSeed
    {
        public static int ID_NOBODY = 1999;
        public static int ID_ROWLING = 1000;
        public static int ID_TOLKIEN = 1001;
        public static int ID_MURAKAMI = 1002;
        public static int ID_COELHO = 1003;
        public static int ID_PALAHNIUK = 1004;
        public static int NEW_ID_MITCHELL = 1100;
        public static int NEW_ID_KING = 1101;
        public static int NEW_ID_TSUTSUI = 1102;
        public static int NEW_ID_KAHNEMAN = 1103;
        public static int REMOVE_ID_NESBO = 1200;

        public static Author ROWLING = new Author
        {
            Id = ID_ROWLING,
            Firstname = "Joanne",
            Middlename = "Kathleen",
            Lastname = "Rowling"
        };

        public static Author TOLKIEN = new Author
        {
            Id = ID_TOLKIEN,
            Firstname = "John",
            Middlename = "Ronald Reuel",
            Lastname = "Tolkien"
        };

        public static Author MURAKAMI = new Author
        {
            Id = ID_MURAKAMI,
            Firstname = "Haruki",
            Lastname = "Murakami"
        };

        public static Author COELHO = new Author
        {
            Id = ID_COELHO,
            Firstname = "Pablo",
            Lastname = "Coelo"
        };

        public static Author PALAHNIUK = new Author
        {
            Id = ID_PALAHNIUK,
            Firstname = "Chuck",
            Lastname = "Palahniuk"
        };

        public static Author MITCHELL = new Author
        {
            Id = NEW_ID_MITCHELL,
            Firstname = "David",
            Lastname = "Mitchell"
        };

        public static Author KING = new Author
        {
            Id = NEW_ID_KING,
            Firstname = "Stephen",
            Lastname = "King"
        };

        public static Author NESBO = new Author
        {
            Id = REMOVE_ID_NESBO,
            Firstname = "Jo",
            Lastname = "Nesbø"
        };

        public static Author TSUTSUI = new Author
        {
            Id = NEW_ID_TSUTSUI,
            Firstname = "Yasutaka",
            Lastname = "Tsutsui"
        };

        public static Author KAHNEMAN = new Author
        {
            Id = NEW_ID_KAHNEMAN,
            Firstname = "Daniel",
            Lastname = "Kahneman"
        };
    }
}

using Idea.Tests.Entity;

namespace Idea.Tests.Fixture.Seed
{
    public class CategorySeed
    {
        public static int ID_NOVEL = 2000;
        public static int ID_FANTASY = 2001;
        public static int ID_MAGICALREALISM = 2002;
        public static int ID_NATURALISM = 2003;
        public static int ID_HOROR = 2004;
        public static int ID_CRIME = 2005;

        public static Category NOVEL = new Category
        {
            Id = ID_NOVEL,
            Name = "Novel"
        };

        public static Category FANTASY = new Category
        {
            Id = ID_FANTASY,
            Name = "Fandasy"
        };

        public static Category MAGICAL_REALISM = new Category
        {
            Id = ID_MAGICALREALISM,
            Name = "Magical realism"
        };

        public static Category NATURALISM = new Category
        {
            Id = ID_NATURALISM,
            Name = "Naturalism"
        };

        public static Category HOROR = new Category
        {
            Id = ID_HOROR,
            Name = "Horor"
        };

        public static Category CRIME = new Category
        {
            Id = ID_CRIME,
            Name = "Crime"
        };
    }
}

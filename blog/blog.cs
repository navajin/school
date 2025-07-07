using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstBlogDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext()) //this creates a new instance of the BloggingContext class, which is used to interact with the database
            {
                //This will Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                Console.Write("\nEnter a title for a new Post: ");
                var title = Console.ReadLine();

                Blog blog = new Blog() { Name = name }; //this is a new Blog opject called blog and we set the name property to the name we just read from the console
                db.Blogs.Add(blog); //this adds the blog object to the Blogs table in the database, if not created yet, it will create the table
                db.SaveChanges(); //this saves the changes to the datebase

                Post post = new Post() { Title = title, Blog = blog }; //this is a new Post object called post and we set the title property to the title we just read from the console
                db.Posts.Add(post); //this adds the post object to the Posts table in the database, if not created yet, it will create the table
                db.SaveChanges(); //this saves the changes to the database

                //Display all Blogs from the database
                var blogQuery = from b in db.Blogs //this is a LINQ query that selects all blogs from the Blogs table
                                orderby b.Name //this orders the blogs 
                                Console.WriteLine("\n\nAll blogs in the database:");
                foreach (var item in blogQuery) //this is a foreach loop that iterates through each blog in the blogQuery
                {
                    Console.WriteLine($"\n{item.Name}"); //this writes the name of each blog to the console on a new line
                }

                var postQuery = from p in db.Posts
                                orderby p.Title
                                select p;

                Console.WriteLine("\n\nAll posts in the database:");
                foreach (var item in postQuery) //this is a foreach loop that iterates through each post in the postQuery
                {
                    Console.WriteLine($"\n{item.Title}"); //this writes the title of each post to the console
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey(); //this waits for a key press before exiting
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; } //this is a navigation property that allows us to access the posts associated with this blog
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; } //this is a navigation property that allows us to access the blog associated with this post
    }

    public class BloggingContext : DbContext //this is the context class that derives from DbContext, which is used to interact with the database
    {
        public DbSet<Blog> Blogs { get; set; } //this is a DbSet property that represents the Blogs table in the database
        public DbSet<Post> Posts { get; set; } //this is a DbSet property that represents the Posts table in the database
    }
}
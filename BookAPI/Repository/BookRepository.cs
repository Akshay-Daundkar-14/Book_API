using BookAPI.Models;
using BookAPI.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data;
using BookAPI.Utility;

namespace BookAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BookCS");
        }

        public void AddBook(Book book)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcNames.AddNewBook, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Genre", book.Genre);
                    cmd.Parameters.AddWithValue("@PublishedYear", book.PublishedYear);
                    cmd.Parameters.AddWithValue("@Price", book.Price);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Book> GetBooks()
        {
            var books = new List<Book>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcNames.GetAllBooks, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book
                            {
                                BookID = (int)reader["BookID"],
                                Title = (string)reader["Title"],
                                Author = (string)reader["Author"],
                                Genre = reader["Genre"] as string,
                                PublishedYear = (int)reader["PublishedYear"],
                                Price = (int)reader["Price"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                UpdatedDate = (DateTime)reader["UpdatedDate"],
                                IsDeleted = (bool)reader["IsDeleted"]
                            });
                        }
                    }
                }
            }
            return books;
        }

        public void UpdateBook(Book book)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcNames.UpdateExistingBook, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookID", book.BookID);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Genre", book.Genre);
                    cmd.Parameters.AddWithValue("@PublishedYear", book.PublishedYear);
                    cmd.Parameters.AddWithValue("@Price", book.Price);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SoftDeleteBook(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcNames.DeleteExistingBook, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookID", bookId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<Book> GetBooks(string genre, string author, string sortColumn, string sortOrder)
        {
            var books = new List<Book>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcNames.GetFilterBooks, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Genre", genre );
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
                    cmd.Parameters.AddWithValue("@SortOrder", sortOrder);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book
                            {
                                BookID = (int)reader["BookID"],
                                Title = (string)reader["Title"],
                                Author = (string)reader["Author"],
                                Genre = reader["Genre"] as string,
                                PublishedYear = (int)reader["PublishedYear"],
                                Price = (int)reader["Price"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                UpdatedDate = (DateTime)reader["UpdatedDate"],
                                IsDeleted = (bool)reader["IsDeleted"]
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

    }
}

using Cricket_Score.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Cricket_Score.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CricketScoreController : ControllerBase
    {
        private readonly string connectionString;



        public CricketScoreController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        
        [HttpGet]
        public ActionResult<IEnumerable<CricketScore>> Get()
        {
            var scores = new List<CricketScore>();



            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM information";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var score = new CricketScore
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Team1 = reader["Team1"].ToString(),
                                Team2 = reader["Team2"].ToString(),
                                Score = Convert.ToInt32(reader["Score"]),
                                // Map other properties as needed
                            };



                            scores.Add(score);
                        }
                    }
                }
            }



            return Ok(scores);
        }



        // GET: api/cricketscore/{id}
        [HttpGet("{id}")]
        public ActionResult<CricketScore> Get(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * from information WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var score = new CricketScore
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                            Team1 = reader["Team1"].ToString(),
                            Team2 = reader["Team2"].ToString(),
                            Score = Convert.ToInt32(reader["Score"]),
                              
                            };



                            return Ok(score);
                        }
                    }
                }
            }



            return NotFound();
        }



        // POST: api/cricketscore
        [HttpPost]
        public ActionResult<CricketScore> Post(CricketScore score)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO information (Id,Team1, Team2, Score) VALUES (@Id,@Team1, @Team2, @Score);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", score.Id);
                    command.Parameters.AddWithValue("@Team1", score.Team1);
                    command.Parameters.AddWithValue("@Team2", score.Team2);
                    command.Parameters.AddWithValue("@Score", score.Score);
                    



                    
                



                    return CreatedAtAction(nameof(Get), new { id = score.Id }, score);
                }
            }
        }



        // PUT: api/cricketscore/{id}
        [HttpPut("{id}")]
        public ActionResult<CricketScore> Put(int id, CricketScore updatedScore)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE information SET Team1 = @Team1, Team2 = @Team2, Score = @Score WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Team1", updatedScore.Team1);
                    command.Parameters.AddWithValue("@Team2", updatedScore.Team2);
                    command.Parameters.AddWithValue("@Score", updatedScore.Score);
                    // Add other parameters as needed



                    var rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        updatedScore.Id = id;
                        return Ok(updatedScore);
                    }
                }
            }



            return NotFound();
        }



        // DELETE: api/cricketscore/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM information WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);



                    var rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return NoContent();
                    }
                }
            }



            return NotFound();
        }
    }
}


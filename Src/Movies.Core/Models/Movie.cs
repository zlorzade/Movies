using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core
{
    public class Movie:IEquatable<Movie>
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string description { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public int GenreId { get; set; }

        public bool Equals(Movie? other)
        {
            if (other == null)
                return false;

            return this.Code.Equals(other.Code) &&
                (other.Id == 0 || this.Id == 0 || this.Id.Equals(other.Id)) &&
                this.IsActive.Equals(other.IsActive) &&
                this.GenreId.Equals(other.GenreId) &&
                (DateTime.Compare(this.LastUpdatedDate, other.LastUpdatedDate) == 0) &&
                (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&
                (
                    object.ReferenceEquals(this.description, other.description) ||
                    this.description != null &&
                    this.description.Equals(other.description)
                );
        }
        public override int GetHashCode() => (Code).GetHashCode();
    }
}

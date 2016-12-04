using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace Migx.Web.Models
{
    public class MigxContext : DbContext
    {
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<TimeLineItemModel> Itens { get; set; }
        public DbSet<FotoModel> Fotos { get; set; }
        public DbSet<EmprestimoModel> Emprestimos { get; set; }

        public DbSet<AmigosModel> Amigos { get; set; }
        public DbSet<SolicitacaoAmizadeModel> SolicitoesAmizade { get; set; }

        public MigxContext() : base("name=MigxContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>(); //Forçar o delete manual dos registros
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); //Forçar o delete manual dos registros

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //return base.SaveChanges();
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var failure in dbEx.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }

                    sb.Append("----------\n");
                    foreach (var error in failure.Entry.CurrentValues.PropertyNames)
                    {
                        sb.AppendFormat("- {0} : {1}", error.ToString(), failure.Entry.CurrentValues.GetValue<Object>(error.ToString()));
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), dbEx
                );
            }
            catch (Exception err)
            {
                throw;
            }
        }
    }
}
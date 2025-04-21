// Domínio
namespace BibliotecaApp.Dominio
{
    public class Livro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public bool Disponivel { get; set; } = true;
    }

    public class Usuario
    {
        public string Nome { get; set; }
        public int ID { get; set; }
    }

    public class Emprestimo
    {
        public Livro Livro { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public DateTime? DataDevolucaoEfetiva { get; set; }

        public double CalcularMulta()
        {
            if (DataDevolucaoEfetiva == null || DataDevolucaoEfetiva <= DataDevolucaoPrevista)
                return 0;

            var diasAtraso = (DataDevolucaoEfetiva.Value - DataDevolucaoPrevista).Days;
            return diasAtraso * 1.0;
        }
    }
}

// Serviços
namespace BibliotecaApp.Servicos
{
    using BibliotecaApp.Dominio;
    using BibliotecaApp.Notificacoes;

    public class BibliotecaService
    {
        private readonly List<Livro> livros = new();
        private readonly List<Usuario> usuarios = new();
        private readonly List<Emprestimo> emprestimos = new();
        private readonly INotificador notificador;

        public BibliotecaService(INotificador notificador)
        {
            this.notificador = notificador;
        }

        public void AdicionarLivro(Livro livro) => livros.Add(livro);

        public void AdicionarUsuario(Usuario usuario)
        {
            usuarios.Add(usuario);
            notificador.Enviar(usuario.Nome, "Bem-vindo à Biblioteca", "Você foi cadastrado em nosso sistema!");
        }

        public bool RealizarEmprestimo(int usuarioId, string isbn, int diasEmprestimo)
        {
            var livro = livros.Find(l => l.ISBN == isbn && l.Disponivel);
            var usuario = usuarios.Find(u => u.ID == usuarioId);

            if (livro == null || usuario == null) return false;

            livro.Disponivel = false;
            var emprestimo = new Emprestimo
            {
                Livro = livro,
                Usuario = usuario,
                DataEmprestimo = DateTime.Now,
                DataDevolucaoPrevista = DateTime.Now.AddDays(diasEmprestimo)
            };
            emprestimos.Add(emprestimo);

            notificador.Enviar(usuario.Nome, "Empréstimo Realizado", $"Você pegou emprestado o livro: {livro.Titulo}");
            return true;
        }

        public double RealizarDevolucao(string isbn, int usuarioId)
        {
            var emprestimo = emprestimos.Find(e => e.Livro.ISBN == isbn && e.Usuario.ID == usuarioId && e.DataDevolucaoEfetiva == null);
            if (emprestimo == null) return -1;

            emprestimo.DataDevolucaoEfetiva = DateTime.Now;
            emprestimo.Livro.Disponivel = true;

            double multa = emprestimo.CalcularMulta();
            if (multa > 0)
            {
                notificador.Enviar(emprestimo.Usuario.Nome, "Multa por Atraso", $"Você tem uma multa de R$ {multa}");
            }

            return multa;
        }

        public List<Livro> BuscarLivros() => livros;
        public List<Usuario> BuscarUsuarios() => usuarios;
        public List<Emprestimo> BuscarEmprestimos() => emprestimos;
    }
}

// Interfaces de Notificação
namespace BibliotecaApp.Notificacoes
{
    public interface INotificador
    {
        void Enviar(string destinatario, string assunto, string mensagem);
    }

    public class EmailNotificador : INotificador
    {
        public void Enviar(string destinatario, string assunto, string mensagem)
        {
            Console.WriteLine($"[EMAIL] Para: {destinatario} | Assunto: {assunto} | Msg: {mensagem}");
        }
    }

    public class SmsNotificador : INotificador
    {
        public void Enviar(string destinatario, string assunto, string mensagem)
        {
            Console.WriteLine($"[SMS] Para: {destinatario} | Msg: {mensagem}");
        }
    }
}

// Programa Principal
namespace BibliotecaApp
{
    using BibliotecaApp.Dominio;
    using BibliotecaApp.Notificacoes;
    using BibliotecaApp.Servicos;

    class Program
    {
        static void Main()
        {
            var notificador = new EmailNotificador();
            var biblioteca = new BibliotecaService(notificador);

            biblioteca.AdicionarLivro(new Livro { Titulo = "Clean Code", Autor = "Robert C. Martin", ISBN = "978-0132350884" });
            biblioteca.AdicionarLivro(new Livro { Titulo = "Design Patterns", Autor = "Erich Gamma", ISBN = "978-0201633610" });

            biblioteca.AdicionarUsuario(new Usuario { Nome = "João Silva", ID = 1 });
            biblioteca.AdicionarUsuario(new Usuario { Nome = "Maria Oliveira", ID = 2 });

            biblioteca.RealizarEmprestimo(1, "978-0132350884", 7);
            double multa = biblioteca.RealizarDevolucao("978-0132350884", 1);
            Console.WriteLine($"Multa por atraso: R$ {multa}");

            Console.ReadLine();
        }
    }
}

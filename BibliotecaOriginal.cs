using System;
using System.Collections.Generic;

namespace BibliotecaApp
{
    // Classe que faz tudo no sistema
    public class GerenciadorBiblioteca
    {
        private List<Livro> livros = new List<Livro>();
        private List<Usuario> usuarios = new List<Usuario>();
        private List<Emprestimo> emprestimos = new List<Emprestimo>();
       
        // Método que adiciona um livro
        public void AdicionarLivro(string titulo, string autor, string isbn)
        {
            var l = new Livro();
            l.Titulo = titulo;
            l.Autor = autor;
            l.ISBN = isbn;
            l.Disponivel = true;
            livros.Add(l);
            Console.WriteLine("Livro adicionado: " + titulo);
        }
       
        // Método que adiciona um usuário
        public void AdicionarUsuario(string nome, int id)
        {
            var u = new Usuario();
            u.Nome = nome;
            u.ID = id;
            usuarios.Add(u);
            Console.WriteLine("Usuário adicionado: " + nome);
           
            // Enviar email de boas-vindas
            EnviarEmail(u.Nome, "Bem-vindo à Biblioteca", "Você foi cadastrado em nosso sistema!");
        }
       
        // Método que realiza empréstimo
        public bool RealizarEmprestimo(int usuarioId, string isbn, int diasEmprestimo)
        {
            var livro = livros.Find(l => l.ISBN == isbn);
            var usuario = usuarios.Find(u => u.ID == usuarioId);
           
            if (livro != null && usuario != null && livro.Disponivel)
            {
                livro.Disponivel = false;
                var emprestimo = new Emprestimo();
                emprestimo.Livro = livro;
                emprestimo.Usuario = usuario;
                emprestimo.DataEmprestimo = DateTime.Now;
                emprestimo.DataDevolucaoPrevista = DateTime.Now.AddDays(diasEmprestimo);
                emprestimos.Add(emprestimo);
               
                // Enviar email sobre empréstimo
                EnviarEmail(usuario.Nome, "Empréstimo Realizado",
                    "Você pegou emprestado o livro: " + livro.Titulo);
               
                // Enviar SMS (misturando responsabilidades)
                EnviarSMS(usuario.Nome, "Empréstimo do livro: " + livro.Titulo);
               
                return true;
            }
           
            return false;
        }
       
        // Método que realiza devolução e calcula multa
        public double RealizarDevolucao(string isbn, int usuarioId)
        {
            var emprestimo = emprestimos.Find(e =>
                e.Livro.ISBN == isbn &&
                e.Usuario.ID == usuarioId &&
                e.DataDevolucaoEfetiva == null);
               
            if (emprestimo != null)
            {
                emprestimo.DataDevolucaoEfetiva = DateTime.Now;
                emprestimo.Livro.Disponivel = true;
               
                // Calcular multa (R$ 1,00 por dia de atraso)
                double multa = 0;
                if (DateTime.Now > emprestimo.DataDevolucaoPrevista)
                {
                    TimeSpan atraso = DateTime.Now - emprestimo.DataDevolucaoPrevista;
                    multa = atraso.Days * 1.0;
                   
                    // Enviar email sobre multa
                    EnviarEmail(emprestimo.Usuario.Nome, "Multa por Atraso",
                        "Você tem uma multa de R$ " + multa);
                }
               
                return multa;
            }
           
            return -1; // Código de erro
        }
       
        // Método para enviar e-mail
        private void EnviarEmail(string destinatario, string assunto, string mensagem)
        {
            // Simulação de envio de e-mail
            Console.WriteLine($"E-mail enviado para {destinatario}. Assunto: {assunto}");
        }
       
        // Método para enviar SMS
        private void EnviarSMS(string destinatario, string mensagem)
        {
            // Simulação de envio de SMS
            Console.WriteLine($"SMS enviado para {destinatario}: {mensagem}");
        }
       
        // Método para buscar todos os livros
        public List<Livro> BuscarTodosLivros()
        {
            return livros;
        }
       
        // Método para buscar todos os usuários
        public List<Usuario> BuscarTodosUsuarios()
        {
            return usuarios;
        }
       
        // Método para buscar todos os empréstimos
        public List<Emprestimo> BuscarTodosEmprestimos()
        {
            return emprestimos;
        }
    }
   
    // Classe de Livro
    public class Livro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public bool Disponivel { get; set; }
    }
   
    // Classe de Usuário
    public class Usuario
    {
        public string Nome { get; set; }
        public int ID { get; set; }
    }
   
    // Classe de Empréstimo
    public class Emprestimo
    {
        public Livro Livro { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public DateTime? DataDevolucaoEfetiva { get; set; }
    }
   
    // Classe Program para testar
    class Program
    {
        static void Main(string[] args)
        {
            var biblioteca = new GerenciadorBiblioteca();
           
            // Adicionar livros
            biblioteca.AdicionarLivro("Clean Code", "Robert C. Martin", "978-0132350884");
            biblioteca.AdicionarLivro("Design Patterns", "Erich Gamma", "978-0201633610");
           
            // Adicionar usuários
            biblioteca.AdicionarUsuario("João Silva", 1);
            biblioteca.AdicionarUsuario("Maria Oliveira", 2);
           
            // Realizar empréstimo
            biblioteca.RealizarEmprestimo(1, "978-0132350884", 7);
           
            // Realizar devolução (com atraso simulado)
            // Note: Em uma aplicação real, você não manipularia as datas desta forma
            double multa = biblioteca.RealizarDevolucao("978-0132350884", 1);
            Console.WriteLine($"Multa por atraso: R$ {multa}");
           
            Console.ReadLine();
        }
    }
}
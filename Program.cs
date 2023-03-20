using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeClientes
{
    internal class Program
    {
        [System.Serializable]
        struct Cliente
        {
            public string nome;
            public string email;
            public string cpf;
        }

        static List<Cliente> clientes = new List<Cliente>();

        enum Menu { Listagem = 1, Adicionar, Remover, Sair }

        static void Main(string[] args)
        {
            Carregar();

            bool escolheuSair = false;
            while(!escolheuSair)
            {
                Console.WriteLine("### SISTEMA DE CLIENTES - BEM VINDO! ###");
                Console.WriteLine("1 - Listagem\n2 - Adicionar\n3 - Remover\n4 - Sair");
                int intOp = int.Parse(Console.ReadLine());
                Menu opcao = (Menu)intOp;

                switch (opcao)
                {
                    case Menu.Listagem:
                        Listagem(false);
                        break;
                    case Menu.Adicionar:
                        Adicionar();
                        break;
                    case Menu.Remover:
                        Remover();
                        break;
                    case Menu.Sair:
                        escolheuSair = true;
                        break;
                }

                Console.Clear();
            }            
        }

        static void Listagem (bool remover)
        {
            Console.Clear();
            if (clientes.Count > 0)
            {
                int i = 0;

                Console.WriteLine("Lista de clientes.");
                Console.WriteLine("#");
                foreach (Cliente cliente in clientes)
                {
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine($"ID:     {i}");
                    Console.WriteLine($"Nome:   {cliente.nome}");
                    Console.WriteLine($"E-mail: {cliente.email}");
                    Console.WriteLine($"CPF:    {cliente.cpf}");
                    i++;
                }
            }
            else
            {
                Console.WriteLine("#");
                Console.WriteLine("Nenhum cliente cadastrado.");
            }

            if (!remover)
            {
                Console.WriteLine("#");
                Console.WriteLine("Aperte ENTER para retornar.");
                Console.ReadLine();
            }            
        }
        static void Adicionar()
        {
            Console.Clear();
            Cliente cliente = new Cliente();
            Console.WriteLine("Cadastro de cliente.");
            Console.WriteLine("#");
            Console.Write("Nome do cliente: ");
            cliente.nome = Console.ReadLine();
            Console.Write("E-mail do cliente: ");
            cliente.email = Console.ReadLine();
            Console.Write("CPF do cliente: ");
            cliente.cpf = Console.ReadLine();

            clientes.Add(cliente);
            Salvar();

            Console.WriteLine("#");
            Console.Write("Cadastro concluído, aperte ENTER para retornar.");
            Console.ReadLine();
        }

        static void Salvar()
        {
            FileStream stream = new FileStream("clients.dat", FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter();

            encoder.Serialize(stream, clientes);

            stream.Close();
        }

        static void Carregar()
        {
            FileStream stream = new FileStream("clients.dat", FileMode.OpenOrCreate);

            try
            {                
                BinaryFormatter encoder = new BinaryFormatter();

                clientes = (List<Cliente>)encoder.Deserialize(stream);

                if(clientes == null)
                {
                    clientes = new List<Cliente>();
                }                
            }
            catch(Exception e)
            {
                clientes = new List<Cliente>();
            }

            stream.Close();
        }

        static bool Remover()
        {
            Console.Clear();
            Console.WriteLine("Remover cliente.");
            Console.WriteLine("#");
            bool inRemover = true;
            Listagem(inRemover);
            Console.WriteLine("-----------------------------------------");
            Console.Write("Digite o ID do cliente que você quer remover: ");
            int id = int.Parse(Console.ReadLine());

            if(id >= 0 && id < clientes.Count)
            {
                clientes.RemoveAt(id);
                Salvar();
            }
            else
            {
                Console.WriteLine("#");
                Console.WriteLine("ID digitado é inválido, tente novamente!");
                Console.ReadLine();
            }
            return inRemover;
        }
    }
}

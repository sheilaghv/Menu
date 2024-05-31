using System;
using System.Collections.Generic;

namespace Menu
{
    class MenuPrincipal
    {
        List<Menu> menu = new List<Menu>();
        int MenuEleg = 1;
        int ItemEleg = 0;
        bool menuActivo = false;

        public MenuPrincipal(Dictionary<string, string[]> menus) // Recorro el diccionario
        {
            int pos = 1;
            foreach (var subMenu in menus)
            {
                menu.Add(new Menu(pos++, subMenu.Key, subMenu.Value));
            }
        }

        public void Dibujar()
        {
            Console.SetCursorPosition(0, 0); // Establece la posición del cursor al inicio de la consola.

            Console.WriteLine(" Elegi una opción: ");

            for (int i = 0; i < menu.Count; i++)
            {
                if (MenuEleg == i + 1)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.WriteLine($"{menu[i].posMenu}. {menu[i].nombreMenu}");
            }

            Console.ResetColor();
            Console.WriteLine("\nEnter para elegir, Esc para salir.");

            // Limpia el resto de la consola si hay más contenido debajo.
            for (int i = menu.Count + 3; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        public void DibujarSubMenu()
        {
            Console.Clear();
            Console.WriteLine($"Menú: {menu[MenuEleg - 1].nombreMenu}");

            for (int i = 0; i < menu[MenuEleg - 1].items.Length; i++)
            {
                if (ItemEleg == i)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.WriteLine($"{i + 1}. {menu[MenuEleg - 1].items[i]}");
            }

            Console.ResetColor();
            Console.WriteLine(" Enter para elegir, Izquierda para volver al menú principal, Esc para salir.");
        }

        public string[] ObtenerSubMenu()
        {
            return menu[MenuEleg - 1].items;
        }

        public bool MenuSeleccionado()
        {
            return menuActivo;
        }

        public string ObtenerOpcionSeleccionada()
        {
            return menu[MenuEleg - 1].items[ItemEleg];
        }

        public void ProcesarEntrada(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (menuActivo)
                        ItemEleg = Math.Max(ItemEleg - 1, 0); //movimiento entre items, se desplaza hacia arriba en caso de poder [A] (ponele que es una flecha hacia arriba)
                    else
                        MenuEleg = Math.Max(MenuEleg - 1, 1);
                    break;
                case ConsoleKey.DownArrow:
                    if (menuActivo)
                        ItemEleg = Math.Min(ItemEleg + 1, menu[MenuEleg - 1].items.Length - 1);  //Movimiento entre items, se desplaza hacia abajo en caso de poder [v]  
                    else
                        MenuEleg = Math.Min(MenuEleg + 1, menu.Count);
                    break;
                case ConsoleKey.Enter: //Ingresa al menu, o al item activo [Intro]
                    menuActivo = !menuActivo;
                    break;
                case ConsoleKey.Escape: //termina el programa, por completo al tocar [ESC]
                    Environment.Exit(0);
                    break;
                case ConsoleKey.LeftArrow: //te devuelve al inicio al apretar la flecha izquierda [<-]
                    if (menuActivo)
                        menuActivo = false;
                    break;
            }
        }
    }

    class Menu
    {
        public string[] items;
        public string nombreMenu;
        public int posMenu;

        public Menu(int posMenu, string nombreMenu, string[] opciones)
        {
            this.items = opciones;
            this.nombreMenu = nombreMenu;
            this.posMenu = posMenu;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string[] menu1 = { "Nuevo Cliente", "Modificar Cliente", "Listar Clientes", "Salir" };
            string[] menu2 = { "Nuevo Producto", "Modificar Producto", "Eliminar Producto", "Listar Producto", "Salir" };
            string[] menu3 = { "", "Modificar Producto", "Eliminar Producto", "Listar Producto", "Salir" };

            var menus = new Dictionary<string, string[]>
            {
                { "Archivo", menu1 }, { "Editar", menu2 }, { "Ver", menu3 }
            };

            MenuPrincipal menu = new MenuPrincipal(menus);

            while (true)
            {
                if (menu.MenuSeleccionado())
                {
                    menu.DibujarSubMenu();
                }
                else
                {
                    menu.Dibujar();
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                menu.ProcesarEntrada(key);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace bfcs
{
    class Program
    {
        static void Main(string[] args)
        {
            bf b = new bf();
            StreamReader stream;
            for (int x = 0; x < args.Length; x++)
            {
                stream = new StreamReader(args[x]);
                b.Init();
                b.Interpretar(stream.ReadToEnd());
            }
            if (args.Length == 0)
                Console.Write("Debes especificar el archivo fuente a interpretar.");
        }
    }

    class bf
    {
        byte[] m;
        int mp,ip;
        System.Collections.Generic.Stack<int> pila;
        Regex opers;

        public bf()
        {
            Init();
        }

        public void Init()
        {//Inicializacion de las variables del interprete
            mp = 0;
            ip = 0;
            m = new byte[9999];
            pila = new Stack<int>();
            opers=new Regex("[\\+\\-\\.\\,\\<\\>\\[\\]]");
        }

        public void Interpretar(string codigo)
        {
            while (ip < codigo.Length)
            {
                //Ejecucion de las instrucciones
                if(opers.IsMatch(codigo[ip].ToString()))
                switch(codigo[ip])
                {
                    case '+':
                        m[mp]++;
                        break;
                    case '-':
                        m[mp]--;
                        break;
                    case '.':
                        Console.Write((char)m[mp]);
                        break;
                    case ',':
                        string ent=Console.ReadLine();
                        if (ent.Length > 0)
                            m[mp] = (byte)ent[0];
                        else
                            m[mp] = 0;
                        break;
                    case '>':
                        mp++;
                        break;
                    case '<':
                        if(mp>0)
                            mp--;
                        break;
                    case '[':   //Comienza una iteración, (la parte un poco mas complicada que el resto)
                        if (m[mp] > 0)  //Si entra, se guarda el puntero de instruccion a donde volver
                            pila.Push(ip);
                        else
                        {   //La iteración no se debe ejecutar, por lo que se busca el cierre del ciclo que se cancela
                            int x = 0;
                            while (true)
                            {
                                if (codigo[ip] == '[')
                                    x++;
                                else if (codigo[ip] == ']')
                                    x--;
                                if (x==0)
                                    break;
                                ip++;   //OJO! cuando x==0 se cumple, esta isntruccion no se ejecuta 
                            }
                        }
                        break;
                    case ']':
                        if (m[mp] > 0)  //Si hay que volver a ejecutar la iteración, regresa a la instruccion guardada -1
                            ip = pila.Pop() - 1;    //se resta uno pues mas abajo se incrementa en uno nuevamente
                        else
                            pila.Pop(); //Si no hay que volver, simplemente se limpia este nivel de la pila
                        break;
                }
                ip++;
            }
        }
    }
}

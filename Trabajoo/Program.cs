class Juego {
    public static void Main() {
        Personaje sacerdote = new Sacerdote("Samson", 30, 5);
        Personaje barbaro = new Barbaro("Dave", 30, 7, 10);
        Equipo tunica = new Armadura(5);
        Equipo hacha = new Arma(6);

        sacerdote.Equipar(tunica);
        barbaro.Equipar(hacha);

        Personaje? ganador = Batalla(barbaro, sacerdote);
        
        if (ganador != null) {
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("¡Nuevo personaje!\n");
            Console.Write("Ingrese el nombre del nuevo personaje: ");
            string nombre = Console.ReadLine()!;
            Console.Write("Ingrese la vida del nuevo personaje: ");
            int vida = int.Parse(Console.ReadLine()!);
            int vidaMinima = 1;
            int vidaMaxima = 30;
            if (vida < vidaMinima) {
                Console.WriteLine($"La vida no puede ser menor que {vidaMinima}. Se establecerá la vida mínima permitida.");
                vida = vidaMinima;
            } else if(vida > vidaMaxima) {
                Console.WriteLine($"La vida no puede ser mayor que {vidaMaxima}. Se establecerá la vida máxima permitida.");
                vida = vidaMaxima;  // Establecer la vida al valor máximo
            }
            Console.Write("Ingrese el ataque del nuevo personaje: ");
            int ataque = int.Parse(Console.ReadLine()!);
            int ataqueMinimo = 3;
            int ataqueMaximo = 10;
            if (ataque < ataqueMinimo) {
                Console.WriteLine($"El ataque no puede ser menor que {ataqueMinimo}, se establecera el ataque minimo por defecto.");
                ataque = ataqueMinimo;
            } else if(ataque > ataqueMaximo) {
                Console.WriteLine($"El ataque no puede ser mayor que {ataqueMaximo}, se establecera el ataque maximo por defecto.");
                ataque = ataqueMaximo;
            }
            Console.WriteLine("");
            Console.WriteLine("Su personaje utilizara una coca-colita como arma");
            Console.WriteLine("");
            Console.Write("Ingrese el poder del arma de su personaje: ");
            int modAtaque = int.Parse(Console.ReadLine()!);
            int poderMinimo = 3;
            int poderMaximo = 10;
            if (modAtaque < poderMinimo) {
                Console.WriteLine($"El poder no puede ser menor que {poderMinimo}. Se establecerá el poder mínimo permitido.");
                modAtaque = poderMinimo;
            } else if(modAtaque >poderMaximo) {
                Console.WriteLine($"El poder no puede ser mayor que {poderMaximo}. Se establecerá el poder maximo permitido.");
                modAtaque = poderMaximo;
            }
            Console.Write("Ingrese la armadura que tendra su personaje: ");
            int modArmadura = int.Parse(Console.ReadLine()!);
            int armaduraMinima = 3;
            int armaduraMaxima = 10;
            if (modArmadura < armaduraMinima) {
                Console.WriteLine($"La armadura no puede ser menor que {armaduraMinima}. Se establecerá la armadura mínima permitida.");
                modArmadura = armaduraMinima;
            } else if(modArmadura > armaduraMaxima) {
                Console.WriteLine($"La armadura no puede ser mayor que {armaduraMaxima}. Se establecerá la armadura maxima permitida.");
                modArmadura = armaduraMaxima;
            }
            
            Equipo equipoNuevo = new Equipo(modAtaque, modArmadura);
            Personaje nuevoPersonaje = new PersonajeAdicional(nombre, vida, ataque);
            nuevoPersonaje.Equipar(equipoNuevo);

            Personaje? ganadorFinal = Batalla(ganador, nuevoPersonaje);

            if (ganadorFinal != null) {
                Console.WriteLine("");
            } else {
                Console.WriteLine("La batalla final terminó en empate.");
            }
        }
    }
    public static Personaje? Batalla(Personaje p1, Personaje p2) {
        Console.WriteLine("¡Comienza la batalla!\n");

        while (p1.EstaVivo() && p2.EstaVivo()) {
            p1.Atacar(p2);
            if (!p2.EstaVivo()) break;

            p2.Atacar(p1);
        }

        if (p1.EstaVivo()) {
            Console.WriteLine($"{p1.GetNombre()} ha ganado la batalla.");
            return p1;
        } else if (p2.EstaVivo()) {
            Console.WriteLine($"{p2.GetNombre()} ha ganado la batalla.");
            return p2;
        } else {
            Console.WriteLine("La batalla terminó en empate.");
            return null;
        }
    }
}

class Equipo {
    public int ModificadorAtaque { get; set; }
    public int ModificadorArmadura { get; set; }

    public Equipo(int ataque, int armadura) {
        ModificadorAtaque = ataque;
        ModificadorArmadura = armadura;
    }

    public int GetModificadorAtaque() {
        return ModificadorAtaque;
    }

    public int GetModificadorArmadura() {
        return ModificadorArmadura;
    }
}class Arma : Equipo {
    public Arma(int ataque) : base(ataque, 0) { }
}

class Armadura : Equipo {
    public Armadura(int armadura) : base(0, armadura) { }
}
class Personaje {
    private string Nombre { get; set; }
    private int Vida { get; set; }
    private int Ataque { get; set; }
    private Equipo? equipo;

    public Personaje? Objetivo { get; set; }

    public Personaje(string nombre, int vida, int ataque) {
        this.Nombre = nombre;
        this.Vida = vida;
        this.Ataque = ataque;
    }

    public string GetNombre() => Nombre;
    public int GetVida() => Vida;
    public int GetAtaque() {
        return Ataque + (equipo != null ? equipo.GetModificadorAtaque() : 0);
    }

    public int GetArmadura() {
        return equipo != null ? equipo.GetModificadorArmadura() : 0;
    }

    public void Equipar(Equipo nuevoEquipo) {
        equipo = nuevoEquipo;
    }

    public virtual void RecibirDanio(int danio) {
        int dañoTotal = Math.Max(danio - GetArmadura(), 1);
        Vida -= dañoTotal;

        Console.WriteLine($"{Nombre} recibe {dañoTotal} puntos de daño.");

        if (Vida <= 0) {
            Console.WriteLine($"{Nombre} ha muerto :(");
        }
    }

    public virtual void Atacar(Personaje objetivo) {
        Console.WriteLine($"{Nombre} ataca a {objetivo.GetNombre()}");
        int daño = GetAtaque();
        objetivo.RecibirDanio(daño);
    }

    public bool EstaVivo() => Vida > 0;
}

class Sacerdote : Personaje {
    public Sacerdote(string nombre, int vida, int ataque)
        : base(nombre, vida, ataque) { }

    public override void RecibirDanio(int danio) {
        Random rand = new Random();
        if (rand.Next(4) == 0) {
            Console.WriteLine($"Las plegarias de {GetNombre()} han sido escuchadas.");
            danio = (int)Math.Ceiling(danio / 2.0);
        }
        base.RecibirDanio(danio);
    }
}

class Barbaro : Personaje {
    public int Furia { get; set; }

    public Barbaro(string nombre, int vida, int ataque, int furia)
        : base(nombre, vida, ataque) {
        Furia = furia;
    }

    public override void Atacar(Personaje objetivo) {
        int daño = GetAtaque();
        if (Furia >= 3) {
            Console.WriteLine($"{GetNombre()} ataca furioso.");
            daño = (int)Math.Ceiling(daño * 1.15);
            Furia -= 3;
        } else {
            Console.WriteLine($"{GetNombre()} está cansado.");
            daño = (int)Math.Ceiling(daño * 0.5);
        }
        objetivo.RecibirDanio(daño);
    }
}

class PersonajeAdicional : Personaje {
    public PersonajeAdicional(string nombre, int vida, int ataque)
        : base(nombre, vida, ataque) { }

    public override void Atacar(Personaje objetivo) {
        Random rand = new Random();
        int daño = GetAtaque();
        if (rand.Next(5) == 0) {
            daño = (int)(daño * 1.2); 
            Console.WriteLine($"{GetNombre()} usa un hechizo poderoso!");
        } else if (rand.Next(5) == 1) {
            daño = (int)(daño * 0.8); 
            Console.WriteLine($"{GetNombre()} pierde el control de su magia!");
        }
        Console.WriteLine($"{GetNombre()} ataca a {objetivo.GetNombre()}");
        objetivo.RecibirDanio(daño);
    }
}
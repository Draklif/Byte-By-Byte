function Footer() {
  return (
    <footer className="bg-gray-800 text-white p-4">
      <div className="max-w-7xl mx-auto grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-16">
        {/* Columna 1: Logo y derechos reservados */}
        <div className="flex flex-col items-center lg:items-start">
          <img src="./LogoUDB.png" alt="Logo Universidad de Boyacá" className="h-12 mb-4" />
          <p className="text-gray-400 text-sm text-center lg:text-left">
            &copy; © 2024 Universidad de Boyacá. Todos los derechos reservados. 
          </p>
          <br />
          <p className="text-gray-400 text-sm text-center lg:text-left">Vigilada Mineducación - Personería Jurídica Nº 6553 de mayo de 1981 - Resolución Nº 2910 del 16 de septiembre de 2004 MEN</p>
        </div>

        {/* Columna 2: Enlaces de interés */}
        <div className="flex flex-col items-center lg:items-start space-y-2">
          <a href="https://www.uniboyaca.edu.co/es" className="hover:text-gray-300">Acerca de</a>
          <a href="https://www.uniboyaca.edu.co/es/facultad/ciencias-e-ingenieria" className="hover:text-gray-300">Facultad de Ciencias e Ingeniería</a>
          <a href="https://www.uniboyaca.edu.co/es/ciencias-e-ingenieria/ingenieria-en-multimedia" className="hover:text-gray-300">Ingeniería en Multimedia</a>
          <a href="https://www.uniboyaca.edu.co/sites/default/files/2024-10/Valores%20Matri%CC%81culas%20-%20Derechos%20Pecuniarios%202025_V13sept.pdf" className="hover:text-gray-300">Derechos pecuniarios 2024</a>
          <a href="https://www.uniboyaca.edu.co/sites/default/files/2019-02/Proteccio%CC%81n%20de%20Datos%20-Acuerdo%201155%20-%202018.pdf" className="hover:text-gray-300">Protección de datos</a>
        </div>

        {/* Columna 3: Más enlaces */}
        <div className="flex flex-col items-center lg:items-start space-y-2">
          <a href="https://www.uniboyaca.edu.co/es/reglamentos-estudiantiles" className="hover:text-gray-300">Reglamentos estudiantiles</a>
          <a href="https://www.uniboyaca.edu.co/sites/default/files/2018-01/Estatuto%20General.pdf" className="hover:text-gray-300">Estatuto general</a>
          <a href="https://www.uniboyaca.edu.co/sites/default/files/2021-08/Procedimiento%20modificaci%C3%B3n%20reglamentos.pdf" className="hover:text-gray-300">Procedimientos para modificación de reglamentos</a>
          <a href="https://www.uniboyaca.edu.co/es/regimen-tributario-especial" className="hover:text-gray-300">Régimen tributario</a>
          <a href="https://www.uniboyaca.edu.co/es/documentos-institucionales-universidad-de-boyaca" className="hover:text-gray-300">Documentos institucionales</a>
        </div>

        {/* Columna 4: Redes sociales */}
        <div className="flex flex-col items-center lg:items-start space-y-2">
          <p className="font-bold">Síguenos en:</p>
          <div className="flex space-x-2">
            <a href="#" className="text-xl hover:text-gray-400">X</a>
            <a href="#" className="text-xl hover:text-gray-400">F</a>
          </div>
        </div>
      </div>
    </footer>
  );
}

export default Footer;

function Footer() {
  return (
    <footer className="bg-gray-800 text-white p-4">
      <div className="max-w-7xl mx-auto flex justify-between items-center flex-wrap">
        <img src="/path_to_logo/logo_ub.svg" alt="Logo Universidad de Boyacá" className="h-12"/>
        <div className="flex space-x-4">
          <a href="#" className="hover:text-gray-300">Acerca de</a>
          <a href="#" className="hover:text-gray-300">Reglamentos estudiantiles</a>
          <a href="#" className="hover:text-gray-300">Estatuto general</a>
          <a href="#" className="hover:text-gray-300">Procedimientos para modificación de reglamentos</a>
          <a href="#" className="hover:text-gray-300">Régimen tributario</a>
          <a href="#" className="hover:text-gray-300">Documentos institucionales</a>
        </div>
        <div className="flex space-x-2">
          <a href="#" className="text-xl hover:text-gray-400">X</a>
          <a href="#" className="text-xl hover:text-gray-400">F</a>
        </div>
      </div>
      <p className="text-center text-gray-400 text-sm mt-4">
        &copy; 2024 Universidad de Boyacá. Todos los derechos reservados.
      </p>
    </footer>
  );
}

export default Footer;

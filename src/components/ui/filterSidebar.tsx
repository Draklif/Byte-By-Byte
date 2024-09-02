import React, { useState } from "react";

function FilterSidebar({ filtrosActivos, categoryCounts, onAddFilter, onRemoveFilter, }) {
  const [filtroInput, setFiltroInput] = useState("");
  const [isCategoriesOpen, setIsCategoriesOpen] = useState(false);

  const agregarFiltro = (e: React.KeyboardEvent<HTMLInputElement>) => {
    const target = e.target as HTMLInputElement;
    if (e.key === "Enter" && target.value) {
      onAddFilter(target.value);
      setFiltroInput("");
    }
  };

  return (
    <div className="w-[700px] p-4 bg-gray-800 rounded-lg shadow mr-4">
      <h2 className="text-xl font-bold text-white mb-3">FILTROS</h2>
      <input
        type="text"
        placeholder="Buscar por nombre o etiqueta..."
        className="w-full p-2 bg-gray-900 rounded mb-2"
        value={filtroInput}
        onChange={(e) => setFiltroInput(e.target.value)}
        onKeyDown={agregarFiltro}
      />
      {filtrosActivos.map((filtro) => (
        <div
          key={filtro}
          className="p-2 rounded flex justify-between items-center mb-1 bg-gray-700"
        >
          {filtro}
          <button
            onClick={() => onRemoveFilter(filtro)}
            className="text-red-500"
          >
            x
          </button>
        </div>
      ))}
      <hr className="my-4 border-gray-700" />
      <h3 className="text-lg font-bold">Categorías</h3>
      <button
        onClick={() => setIsCategoriesOpen(!isCategoriesOpen)}
        className="text-sm text-blue-400"
      >
        {isCategoriesOpen ? "Ocultar" : "Mostrar"}
      </button>
      {isCategoriesOpen && (
        <ul className="mt-2">
          {Object.entries(categoryCounts).map(([tag, count]) => (
            <li
              key={tag}
              className="flex justify-between cursor-pointer hover:bg-gray-700 p-1 rounded"
              onClick={() => onAddFilter(tag)}
            >
              {tag} <span className="text-blue-300">{count}</span>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default FilterSidebar;

import { useState, useEffect } from "react";
import { createFileRoute } from "@tanstack/react-router";
import data from "@/data/data.json";
import FilterSidebar from "@/components/ui/filterSidebar";
import ProjectList from "@/components/reusables/projectList";
import { Project } from "@/types";

export const Route = createFileRoute("/repositorio")({
  component: RepositoryList,
});

const projects: Project[] = data.projects;
const itemsPerPage = 5;

function RepositoryList() {
  const [filtrosActivos, setFiltrosActivos] = useState<string[]>([]);
  const [proyectos, setProyectos] = useState<Project[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [numPages, setNumPages] = useState(0);
  const [categoryCounts, setCategoryCounts] = useState<Record<string, number>>({});

  useEffect(() => {
    const filteredProjects = projects.filter((p) =>
      p.format.some((fmt) => fmt !== "")
    );
    setProyectos(filteredProjects);
  }, []);

  useEffect(() => {
    const filteredProjects = proyectos.filter((p) =>
      filtrosActivos.every(
        (filtro) =>
          p.name.toLowerCase().includes(filtro) ||
          p.tags.some((tag) => tag.toLowerCase().includes(filtro))
      )
    );

    const counts : Record<string, number> = {};
      filteredProjects.forEach(project => {
        project.tags.forEach(tag => {
          counts[tag] = counts[tag] ? counts[tag] + 1 : 1;
        });
      });

    setCategoryCounts(counts);
    setNumPages(Math.ceil(filteredProjects.length / itemsPerPage));
    setCurrentPage(1); // Reinicia la página cada vez que cambian los filtros
  }, [filtrosActivos, proyectos]);

  const agregarFiltroClick = (filtro: string) => {
    if (!filtrosActivos.includes(filtro.toLowerCase())) {
      setFiltrosActivos([...filtrosActivos, filtro.toLowerCase()]);
    }
  };

  const eliminarFiltro = (filtro: string) => {
    setFiltrosActivos(filtrosActivos.filter((f) => f !== filtro));
  };

  const startIndex = (currentPage - 1) * itemsPerPage;
  const currentProjects = proyectos
    .filter((p) =>
      filtrosActivos.every(
        (filtro) =>
          p.name.toLowerCase().includes(filtro) ||
          p.tags.some((tag) => tag.toLowerCase().includes(filtro))
      )
    )
    .slice(startIndex, startIndex + itemsPerPage);

  return (
    <div className="bg-gray-900 min-h-screen text-white p-6">
      <div className="w-full md:w-4/5 mx-auto">
        <h1 className="text-2xl font-bold mb-2 text-left">Repositorio</h1>
        <div className="flex">
          <FilterSidebar filtrosActivos={filtrosActivos} setFiltrosActivos={setFiltrosActivos}
                         categoryCounts={categoryCounts} onAddFilter={agregarFiltroClick}
                         onRemoveFilter={eliminarFiltro} />
          <ProjectList projects={currentProjects} numPages={numPages} currentPage={currentPage} setCurrentPage={setCurrentPage} />
        </div>
      </div>
    </div>
  );
}
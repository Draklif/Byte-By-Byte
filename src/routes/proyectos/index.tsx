import React, { useState, useEffect } from "react";
import { createFileRoute } from "@tanstack/react-router";
import data from "@/data/data.json";
import ProjectList from "@/components/reusables/projectList";
import CategoryCard from "@/components/reusables/categoryCard";
import { Project } from "@/types";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel";

export const Route = createFileRoute("/proyectos/")({
  component: MainProjectList,
});

const projects: Project[] = data.projects;
const itemsPerPage = 5;

function MainProjectList() {
  const [currentPage, setCurrentPage] = useState(1);
  const [numPages, setNumPages] = useState(0);
  const [filteredProjects, setFilteredProjects] = useState<Project[]>([]);
  const [proyectos, setProyectos] = useState<Project[]>([]);
  const [selectedCategory, setSelectedCategory] = useState("TODOS");

  useEffect(() => {
    const initialProjects = projects.filter(
      (p) => p.format.length === 0 || p.format.every((fmt) => fmt === "")
    );
    setProyectos(initialProjects);
    setFilteredProjects(initialProjects);
    setNumPages(Math.ceil(initialProjects.length / itemsPerPage));
  }, []);

  useEffect(() => {
    if (selectedCategory !== "TODOS") {
      const categoryFilteredProjects = proyectos.filter((p) =>
        p.tags.includes(selectedCategory)
      );
      setFilteredProjects(categoryFilteredProjects);
      setNumPages(Math.ceil(categoryFilteredProjects.length / itemsPerPage));
    } else {
      setFilteredProjects(proyectos);
      setNumPages(Math.ceil(proyectos.length / itemsPerPage));
    }
    setCurrentPage(1);
  }, [selectedCategory, proyectos]);

  const categories = React.useMemo(() => {
    const categoryMap = new Map([["TODOS", projects[0]?.images[0] || ""]]);
    filteredProjects.forEach(project => {
      project.tags.forEach(tag => {
        if (!categoryMap.has(tag)) {
          categoryMap.set(tag, project.images[0]);
        }
      });
    });
    return Array.from(categoryMap, ([tag, image]) => ({ tag, image }));
  }, [filteredProjects]);

  const startIndex = (currentPage - 1) * itemsPerPage;
  const currentProjects = filteredProjects.slice(startIndex, startIndex + itemsPerPage);

  return (
    <div className="bg-gray-900 min-h-screen text-white p-6">
      <div className="w-full md:w-4/5 mx-auto">
        <h1 className="text-2xl font-bold mb-2 text-left">Proyectos</h1>
        <h2 className="text-xl font-semibold mb-4 text-left">
          Explorar Categorías
        </h2>
        <div className="h-64 bg-gray-800 rounded-lg mb-8 flex items-center justify-center">
          <Carousel opts={{ align: "start", loop: true }} className="w-4/5">
            <CarouselContent className="-ml-1">
              {categories.map((category) => (
                <CarouselItem key={category.tag} className="basis-1/5">
                  <CategoryCard
                    category={category.tag}
                    backgroundImage={category.image}
                    onClick={() => setSelectedCategory(category.tag)}
                    isActive={selectedCategory === category.tag}
                  />
                </CarouselItem>
              ))}
            </CarouselContent>
            <CarouselPrevious className="text-black" />
            <CarouselNext className="text-black" />
          </Carousel>
        </div>
        <ProjectList
          projects={currentProjects}
          numPages={numPages}
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
        />
      </div>
    </div>
  );
}

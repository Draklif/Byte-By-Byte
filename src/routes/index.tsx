import MainModel from "@/components/models/mainModel";
import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useState, useEffect } from "react";
import data from "@/data/data.json";
import CategoryCard from "@/components/reusables/categoryCard";
import FeaturedCard from "@/components/reusables/featuredCard";
import { Project } from "@/types";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel";

export const Route = createFileRoute("/")({
  component: Landing,
});

const projects: Project[] = data.projects;

function Landing() {
  const router = useNavigate();
  const [categories, setCategories] = useState([]);
  const [proyectos, setProyectos] = useState<Project[]>([]);

  useEffect(() => {
    const initialProjects = projects.filter(
      (p) => p.format.length === 0 || p.format.every((fmt) => fmt === "")
    );
    setProyectos(initialProjects);

    const categoryMap = new Map();
    proyectos.forEach((project) => {
      project.tags.forEach((tag) => {
        if (!categoryMap.has(tag)) {
          categoryMap.set(tag, project.images[0]);
        }
      });
    });
    setCategories(Array.from(categoryMap, ([tag, image]) => ({ tag, image })));
  });

  const handleCategorySelect = (category: string) => {
    router({ to: `/proyectos?category=${encodeURIComponent(category)}` });
  };

  return (
    <div className="bg-gray-900 min-h-screen text-white p-6">
      <MainModel />
      <h2 className="text-2xl font-bold mt-8 mb-4">Últimos Proyectos</h2>
      <div className="rounded-lg bg-gray-800 mb-8 flex items-center justify-center p-4">
        <Carousel className="w-[1440px]">
          <CarouselContent>
            <CarouselItem>
              {proyectos[0] && <FeaturedCard project={proyectos[0]} />}
            </CarouselItem>
            <CarouselItem>
              {proyectos[1] && <FeaturedCard project={proyectos[1]} />}
            </CarouselItem>
          </CarouselContent>
          <CarouselPrevious className="text-black" />
          <CarouselNext className="text-black" />
        </Carousel>
      </div>
      <h1 className="text-3xl font-bold mt-8">Más Información</h1>
      <p className="text-white mt-4 mb-8">
        Este portal web está diseñado para todos aquellos interesados no
        solamente en crear y aportar al catálogo creciente de proyectos
        multimedia de la Universidad de Boyacá, sino también para quienes deseen
        ver y explorar los diferentes productos que en Ingeniería en Multimedia
        queremos ofrecer.
      </p>
      <p className="text-blue-400 italic">
        ¡Únete ahora a la carrera del futuro y sé parte de los mejores!
      </p>
      <h2 className="text-2xl font-bold mt-8 mb-4">Explora Más</h2>
      <div className="rounded-lg bg-gray-800 mb-8 flex items-center justify-center p-4">
        <Carousel opts={{ align: "start", loop: true }} className="w-4/5">
          <CarouselContent className="-ml-1">
            {categories.map((category) => (
              <CarouselItem key={category.tag} className="basis-1/5">
                <CategoryCard
                  category={category.tag}
                  backgroundImage={category.image}
                  onClick={() => handleCategorySelect(category.tag)}
                  isActive={false}
                />
              </CarouselItem>
            ))}
          </CarouselContent>
          <CarouselPrevious className="text-black" />
          <CarouselNext className="text-black" />
        </Carousel>
      </div>
    </div>
  );
}

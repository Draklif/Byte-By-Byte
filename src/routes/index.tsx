import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useState, useEffect } from "react";
import data from "@/data/data.json";
import ArticleCard from "@/components/reusables/articleCard";
import FeaturedCard from "@/components/reusables/featuredCard";
import Hero from "@/components/reusables/hero";
import { Article } from "@/types";
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

const articles: Article[] = data.articles;


function Landing() {
  const router = useNavigate();
  const [articles, setArticles] = useState<Article[]>([]);

  useEffect(() => {
    // Cargar artículos desde data.json
    setArticles(data.articles);
  }, []);

  const backgroundImage = "public/Default.png"; // Ruta a la imagen de fondo
  const coverImage = "public/Image.png"; // Ruta a la imagen de portada en vertical
  const videoTitle = "Byte by Byte: The Animation"; // Título del video
  const videoUrl = "/video-url"; // URL para redirigir al video

  return (
    <div className="min-h-screen text-white p-6" style={{background: "#202020"}}>
  <Hero
    backgroundImage={backgroundImage}
    coverImage={coverImage}
    videoTitle={videoTitle}
    videoUrl={videoUrl}
    glowColor="#444543"
  />
  <div className="max-w-7xl mx-auto p-4 mt-8">
    <h2 className="text-2xl font-bold mt-8 mb-4"><span className="mx-2">•</span>Últimas Noticias</h2>
    <div className="relative rounded-lg mb-8 flex items-center justify-center py-4">
      <Carousel opts={{ align: "start", loop: false }} className="w-full relative">
        <CarouselContent>
          {articles.map((article) => (
            <CarouselItem key={article.id} className="basis-1/4">
              <FeaturedCard article={article}/>
            </CarouselItem>
          ))}
        </CarouselContent>
        {/* Botones Absolutos en la Esquina Superior Derecha */}
        <CarouselPrevious className="absolute -top-12 right-12 text-black" />
        <CarouselNext className="absolute -top-12 right-0 text-black" />
      </Carousel>
    </div>
    {articles.map((article) => (
      <ArticleCard key={article.id} article={article}></ArticleCard>
    ))}
  </div>
</div>

  );
}

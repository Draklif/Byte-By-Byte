import { Article } from "@/types";

interface FeaturedCardProps {
  article: Article;
}

function FeaturedCard({ article }: FeaturedCardProps) {
  return (
    <div className="overflow-hidden">
      <div className="w-full" style={{ aspectRatio: "16 / 9" }}>
        <img
          src={article.image}
          alt={article.name}
          className="w-full h-full object-cover rounded-lg"
        />
      </div>
      <div className="py-2">
        <h3 className="text-md font-medium text-white">{article.name}</h3>
        <div className="mt-4">
          <span
            onClick={() => {
              window.location.href = article.url;
            }}
            className="cursor-pointer hover:underline"
            style={{color: "#d0eacf"}}
          >
            Ir ahora
          </span>
        </div>
      </div>
    </div>
  );
}

export default FeaturedCard;

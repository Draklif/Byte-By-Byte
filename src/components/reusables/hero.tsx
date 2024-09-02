import { useNavigate } from "@tanstack/react-router";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlay } from '@fortawesome/free-solid-svg-icons';


interface HeroProps {
  backgroundImage: string;
  coverImage: string;
  videoTitle: string;
  videoUrl: string;
  glowColor: string;
}

function Hero({ backgroundImage, coverImage, videoTitle, videoUrl, glowColor }: HeroProps) {
  const router = useNavigate();

  return (
    <div className="relative w-[70%] h-[400px] md:h-[600px] bg-cover bg-center mx-auto rounded-lg"
      style={{
        backgroundImage: `url(${backgroundImage})`,
        backgroundPosition: 'center',
        boxShadow: `0 0 20px 5px ${glowColor}`
      }}>
      <div className="absolute inset-0 bg-gradient-to-b from-transparent to-black/70 to-90% rounded-lg"></div>

      <div className="relative h-full flex items-end justify-center p-4 transform -translate-x-32">
        <div className="relative flex-shrink-0 w-1/5 h-auto transform translate-y-12 shadow-lg">
          <img
            src={coverImage}
            alt={videoTitle}
            className="w-full h-auto object-cover rounded-lg"
            style={{ aspectRatio: "9 / 16" }}
          />
        </div>
        <div className="ml-8 text-white flex items-center">
          <button
            onClick={() => router({ to: videoUrl })}
            className="mr-4 text-white font-bold p-8 px-9 rounded-full flex items-center justify-center"
            style={{
              backgroundColor: "#495154",
              opacity: "60%"
            }}
          >
            <FontAwesomeIcon icon={faPlay} size="2x" color="#c3c3c3"/>
          </button>
          <div>
            <h2 className="text-2xl md:text-4xl font-bold">{videoTitle}</h2>
            <p className="text-lg md:text-xl mt-2">Míralo gratis ahora en nuestro canal oficial</p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Hero;

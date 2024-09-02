import { useFBX, OrbitControls } from "@react-three/drei";
import { Canvas } from "@react-three/fiber";
import { useNavigate } from "@tanstack/react-router";
import { useState } from "react";

function MainModel() {
  const model = useFBX("/Universidad.fbx");
  const router = useNavigate();
  const [moving, setMoving] = useState(false);

  const handleMouseDown = () => {
    setMoving(false);
  };

  const handleMouseMove = () => {
    setMoving(true);
  };

  const handleMouseUp = () => {
    if (!moving) {
      router({ to: "/proyectos/$proyectoId", params: { proyectoId: "1" } });
    }
  };

  return (
    <Canvas
      camera={{ far: 10000, position: [0, 0, 500] }}
      onMouseDown={handleMouseDown}
      onMouseMove={handleMouseMove}
      onMouseUp={handleMouseUp}
    >
      <ambientLight intensity={1} />
      <primitive object={model} scale={0.025} position={[-200, -25, -150]} />
      <OrbitControls />
    </Canvas>
  );
}

export default MainModel;

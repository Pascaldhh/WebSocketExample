export const canvas = document.getElementById("canvas-lobby") as HTMLCanvasElement;
export const ctx = canvas.getContext("2d") as CanvasRenderingContext2D;
export const canvasWidth = 800;
export const canvasHeight = 500;
function setCanvasSize() {
  canvas.width = canvasWidth;
  canvas.height = canvasHeight;

  // Fix blur higher resolution
  if (window.devicePixelRatio > 1) {
    const canvasWidth = canvas.width;
    const canvasHeight = canvas.height;

    canvas.width = canvasWidth * window.devicePixelRatio;
    canvas.height = canvasHeight * window.devicePixelRatio;
    canvas.style.width = canvasWidth + "px";
    canvas.style.height = canvasHeight + "px";

    ctx.scale(window.devicePixelRatio, window.devicePixelRatio);
  }
}

setCanvasSize();

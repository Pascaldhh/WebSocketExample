export function round(num : number, precision: number = 1){
  if(precision <= 0) return Math.round(num);

  const nums = Array(precision)
    .fill(10)
    .reduce((a, b) => a*b);
  return Math.round(num*nums)/nums;
}

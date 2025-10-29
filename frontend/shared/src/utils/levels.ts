import type { Level } from "../api";

export const computeLevel = ({
  levels,
  points,
}: {
  levels?: Level[];
  points: number;
}) => {
  if (!levels) {
    return;
  }

  const level =
    points >= levels?.at(-1)?.pointsCapacity!
      ? levels.at(-1)
      : levels?.find((v) => points <= v.pointsCapacity);

  if (!level) {
    return;
  }

  return {
    levelNumber: levels.findIndex((v) => v === level) + 1,
    currentPoints:
      points > level.pointsCapacity ? level.pointsCapacity : points,
    pointsCapacity: level.pointsCapacity,
  };
};

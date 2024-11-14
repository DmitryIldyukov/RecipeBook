import { jwtDecode } from "jwt-decode";

type JwtPayload = {
  userId: number;
};

export const getUserIdFromToken = (token: string): number | null => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    return decoded.userId || null;
  } catch (error) {
    console.error("Error decoding token:", error);
    return null;
  }
};

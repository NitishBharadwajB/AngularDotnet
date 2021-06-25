import { Photo } from "./photo";

export interface Member {
    id: number;
    username: string;
    photoUrl: string;
    passwordSalt: string;
    age: number;
    createdAt: Date;
    lastActive: Date;
    knownAs: string;
    gender: string;
    introduction: string;
    lookingFor: string;
    intrests?: any;
    city: string;
    country: string;
    photos: Photo[];
}

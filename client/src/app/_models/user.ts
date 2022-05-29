export interface User {
    userId: number;
    firstName: string;
    lastName: string;
    initials: string;
    username: string;
    token: string;
    roles: string[];
}
export interface UserResponse {
    firstName: string
    lastName: string
}
export interface TokenResponse {
    token: string;
    expiration: string;
}

export interface ErrorResponse {
    type: string;
    title: string;
    message?: string;
    status: number;
    traceId?: string;
    errors?: object;
}

export type KnownErrors =
    "USER_DOES_NOT_EXIST"
    | "USER_ALREADY_EXISTS"
    | "INCORRECT_PASSWORD"
    | "PASSWORD_NOT_CONTAINS_LETTERS"
    | "PASSWORD_NOT_CONTAINS_SPECIAL_CHARACTERS"
    | "PASSWORD_TOO_SHORT";

export interface AnswerResponse {
    correct: boolean;
}
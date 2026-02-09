const STORAGE_KEY = "accessToken";
let accessToken = null;

export const tokenManager = {
    set(token){
        accessToken = token;
        if (token) {
            localStorage.setItem(STORAGE_KEY, token);
        } else {
            localStorage.removeItem(STORAGE_KEY);
        }
    },
    get(){
        if (!accessToken) {
            accessToken = localStorage.getItem(STORAGE_KEY);
        }
        return accessToken;
    },
    clear(){
        accessToken = null;
        localStorage.removeItem(STORAGE_KEY);
    }
};

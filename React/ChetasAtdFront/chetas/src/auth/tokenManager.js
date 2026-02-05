let accessToken = null;

export const tokenManager = {
    set(token){
        accessToken = token;
    },
    get(){
        return accessToken;
    },
    clear(){
        accessToken = null;
    }
};
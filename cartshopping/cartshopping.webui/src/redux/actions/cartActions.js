import * as actionTypes from "./actionTypes";
import apiFetch from "./apiFetch";

/** CRUDs  */

export function createCartSuccess(cart) {
    return { type: actionTypes.CREATE_ROLE_SUCCESS, payload: cart }
}
export function updateCartSuccess(cart) {
    return { type: actionTypes.UPDATE_ROLE_SUCCESS, payload: cart }
}
export function deleteCartSuccess(cart) {
    return { type: actionTypes.DELETE_ROLE_SUCCESS, payload: cart }
}
export const getCartsSuccess = (carts) => {
    return { type: actionTypes.GET_ROLES_SUCCESS, payload: carts };
}

export function getCartByIdSuccess(cart) {
    return { type: actionTypes.GET_ROLE_BY_ID_SUCCESS, payload: cart }
}

export const getCarts = () => {
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/Cart/GetCarts';
        await apiFetch(url, 'GET').then(data => {
            if (data.isSuccess) {
                return dispatch(getRolesSuccess(data.result))
            }
            else {
                var error = data.message;
                console.log("getRoles ", error);
            }
        }).catch(handleError)
    }
}
export const getRoleById = (cartId) => {
    console.log("getRoleById ", cartId);
    return async function (dispatch) {
        var url = 'https://localhost:5086/api/Cart/getCartById/' + cartId;
        apiFetch(url).then(data => {
            if (data.isSuccess) {
                return dispatch(getRoleByIdSuccess(data.result))
            }
            else {
                var error = data.message;
                console.log("getRoleById ", error);
            }
        }).catch(handleError);
    }
}
export const createRole = (cart) => {
    debugger;
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/Cart/CreCartole';
        await apiFetch(url, 'POST', cart)
            .then(data => {

                if (data.isSuccess) {
                    return dispatch(createCart
                   Success(data.result))
                }
                else {
                    var error = data.message;
                    console.log("createRole ", error);
                }
            }).catch(handleError);
    }
}
export const updateRole = (cart) => {
    debugger;
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/Cart/UpdCartole';
        await apiFetch(url, 'PUT', cart)
            .then(data => {
                if (data.isSuccess) {
                    return dispatch(updateCart
                   Success(data.result))
                }
                else {
                    var error = data.message;
                    console.log("updateRole ", error);
                }
            }).then(handleError);
    }
}
export const deleteRole = (cart) => {
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/Cart/DelCartole';
        await apiFetch(url, 'DELETE', cart)
            .then(data => {
                if (data.isSuccess) {
                    return dispatch(deleteCart
                   Success(data.result))
                }
                else {
                    var error = data.message;
                    console.log("deleteRole ", error);
                }
            }).catch(handleError);
    }
}
/** Errors  */
export async function handleResponse(response) {
    if (response.ok) {
        return response.json();
    }
    else {
        const error = await response.text()
        throw new Error(error);
    }
}
export function handleError(error) {
    console.error("Resulted error : " + error);
    throw error;
}
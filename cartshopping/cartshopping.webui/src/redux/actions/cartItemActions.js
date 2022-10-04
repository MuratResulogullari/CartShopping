import * as actionTypes from "./actionTypes";
import apiFetch from "./apiFetch";

/** CRUDs  */

export function createCartSuccess(role) {
    return { type: actionTypes.CREATE_ROLE_SUCCESS, payload: role }
}
export function updateCartSuccess(role) {
    return { type: actionTypes.UPDATE_ROLE_SUCCESS, payload: role }
}
export function deleteCartSuccess(role) {
    return { type: actionTypes.DELETE_ROLE_SUCCESS, payload: role }
}
export const getCartsSuccess = (roles) => {
    return { type: actionTypes.GET_ROLES_SUCCESS, payload: roles };
}

export function getCartByIdSuccess(role) {
    return { type: actionTypes.GET_ROLE_BY_ID_SUCCESS, payload: role }
}

export const getCarts = () => {
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/User/GetRoles';
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
export const getRoleById = (roleId) => {
    console.log("getRoleById ", roleId);
    return async function (dispatch) {
        var url = 'https://localhost:5086/api/User/getRoleById/' + roleId;
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
export const createRole = (role) => {
    debugger;
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/User/CreateRole';
        await apiFetch(url, 'POST', role)
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
export const updateRole = (role) => {
    debugger;
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/User/UpdateRole';
        await apiFetch(url, 'PUT', role)
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
export const deleteRole = (role) => {
    return async (dispatch) => {
        var url = 'https://localhost:5086/api/User/DeleteRole';
        await apiFetch(url, 'DELETE', role)
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
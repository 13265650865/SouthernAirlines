import request from '/@/utils/request';
enum Api {
	AddPlaneFleet = '/api/planeFleet/add',
	DeletePlaneFleet = '/api/planeFleet/delete',
	UpdatePlaneFleet = '/api/planeFleet/update',
	PagePlaneFleet = '/api/planeFleet/page',
	ListPlaneFleet = '/api/planeFleet/list',
	UploadPlaneFleet = '/api/planeFleet/upload',
	GetUploadTemplatePlaneFleet = '/api/planeFleet/getUploadTemplate',
}

// 增加机队
export const addPlaneFleet = (params?: any) =>
	request({
		url: Api.AddPlaneFleet,
		method: 'post',
		data: params,
	});

// 删除机队
export const deletePlaneFleet = (params?: any) =>
	request({
		url: Api.DeletePlaneFleet,
		method: 'post',
		data: params,
	});

// 编辑机队
export const updatePlaneFleet = (params?: any) =>
	request({
		url: Api.UpdatePlaneFleet,
		method: 'post',
		data: params,
	});

// 分页查询机队
export const pagePlaneFleet = (params?: any) =>
	request({
		url: Api.PagePlaneFleet,
		method: 'post',
		data: params,
	});

// 机队列表
export const listPlaneFleet = (params?: any) =>
	request({
		url: Api.ListPlaneFleet,
		method: 'get',
		data: params,
	});

export const uploadPlaneFleet = (formData: FormData) =>
	request({
		url: Api.UploadPlaneFleet,
		method: 'post',
		data: FormData,
	});

export const getUploadTemplatePlaneFleet = () =>
	request({
		url: Api.GetUploadTemplatePlaneFleet,
		method: 'get',
		responseType: 'blob'
	});
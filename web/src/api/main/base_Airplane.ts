import request from '/@/utils/request';
enum Api {
	AddBase_Airplane = '/api/plane/add',
	DeleteBase_Airplane = '/api/plane/delete',
	UpdateBase_Airplane = '/api/plane/update',
	PageBase_Airplane = '/api/plane/page',
	GetPlane = '/api/plane/detail',
	ListPlane = '/api/plane/list',
	GetUploadTemplatePlane = '/api/plane/getUploadTemplate',
	PagePlaneComponent = '/api/plane/pagePlaneComponent'
}

// 增加飞机设置
export const addBase_Airplane = (params?: any) =>
	request({
		url: Api.AddBase_Airplane,
		method: 'post',
		data: params,
	});

// 删除飞机设置
export const deleteBase_Airplane = (params?: any) =>
	request({
		url: Api.DeleteBase_Airplane,
		method: 'post',
		data: params,
	});

// 编辑飞机设置
export const updateBase_Airplane = (params?: any) =>
	request({
		url: Api.UpdateBase_Airplane,
		method: 'post',
		data: params,
	});

// 分页查询飞机设置
export const pageBase_Airplane = (params?: any) =>
	request({
		url: Api.PageBase_Airplane,
		method: 'post',
		data: params,
	});

export const getPlane = (params?: any) =>
	request({
		url: Api.GetPlane,
		method: 'get',
		data: params,
	});

export const listPlane = (params?: any) =>
	request({
		url: Api.ListPlane,
		method: 'post',
		data: params,
	});

export const getUploadTemplatePlane = () => {
	return request({
		url: Api.GetUploadTemplatePlane,
		method: 'get',
		responseType: 'blob'
	});
}

export const pagePlaneComponent = (params?: any) =>
	request({
		url: Api.PagePlaneComponent,
		method: 'post',
		data: params,
	});

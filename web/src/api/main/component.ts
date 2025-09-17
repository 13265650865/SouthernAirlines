import request from '/@/utils/request';
enum Api {
	AddComponent = '/api/component/add',
	DeleteComponent = '/api/component/delete',
	UpdateComponent = '/api/component/update',
	PageComponent = '/api/component/page',
	ListComponent = '/api/component/list',
	GetUploadTemplateComponent = '/api/component/GetUploadTemplate',
}

// 增加部件
export const addComponent = (params?: any) =>
	request({
		url: Api.AddComponent,
		method: 'post',
		data: params,
	});

// 删除部件
export const deleteComponent = (params?: any) =>
	request({
		url: Api.DeleteComponent,
		method: 'post',
		data: params,
	});

// 编辑部件
export const updateComponent = (params?: any) =>
	request({
		url: Api.UpdateComponent,
		method: 'post',
		data: params,
	});

// 分页查询部件
export const pageComponent = (params?: any) =>
	request({
		url: Api.PageComponent,
		method: 'post',
		data: params,
	});

export const listComponent = (params?: any) =>
	request({
		url: Api.ListComponent,
		method: 'get',
		data: params,
	});

export const getUploadTemplateComponent = () =>
	request({
		url: Api.GetUploadTemplateComponent,
		method: 'get',
		responseType: 'blob'
	});

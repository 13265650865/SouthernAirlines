import request from '/@/utils/request';
enum Api {
	AddComponentCategory = '/api/componentCategory/add',
	DeleteComponentCategory = '/api/componentCategory/delete',
	UpdateComponentCategory = '/api/componentCategory/update',
	PageComponentCategory = '/api/componentCategory/page',
	ListComponentCategory = '/api/componentCategory/list',
}

// 增加部件类别
export const addComponentCategory = (params?: any) =>
	request({
		url: Api.AddComponentCategory,
		method: 'post',
		data: params,
	});

// 删除部件类别
export const deleteComponentCategory = (params?: any) =>
	request({
		url: Api.DeleteComponentCategory,
		method: 'post',
		data: params,
	});

// 编辑部件类别
export const updateComponentCategory = (params?: any) =>
	request({
		url: Api.UpdateComponentCategory,
		method: 'post',
		data: params,
	});

// 分页查询部件类别
export const pageComponentCategory = (params?: any) =>
	request({
		url: Api.PageComponentCategory,
		method: 'post',
		data: params,
	});

// 部件类别列表
export const listComponentCategory = (params?: any) =>
	request({
		url: Api.ListComponentCategory,
		method: 'get',
		data: params,
	});

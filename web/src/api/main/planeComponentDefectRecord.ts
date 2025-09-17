import request from '/@/utils/request';
enum Api {
	AddPlaneComponentDefectRecord = '/api/planeComponentDefectRecord/add',
	DeletePlaneComponentDefectRecord = '/api/planeComponentDefectRecord/delete',
	UpdatePlaneComponentDefectRecord = '/api/planeComponentDefectRecord/update',
	PagePlaneComponentDefectRecord = '/api/planeComponentDefectRecord/page',
	GetUploadTemplatePlaneComponentDefectRecord = '/api/planeComponentDefectRecord/GetUploadTemplate',
}

// 增加飞机部件缺陷记录
export const addPlaneComponentDefectRecord = (params?: any) =>
	request({
		url: Api.AddPlaneComponentDefectRecord,
		method: 'post',
		data: params,
	});

// 删除飞机部件缺陷记录
export const deletePlaneComponentDefectRecord = (params?: any) =>
	request({
		url: Api.DeletePlaneComponentDefectRecord,
		method: 'post',
		data: params,
	});

// 编辑飞机部件缺陷记录
export const updatePlaneComponentDefectRecord = (params?: any) =>
	request({
		url: Api.UpdatePlaneComponentDefectRecord,
		method: 'post',
		data: params,
	});

// 分页查询飞机部件缺陷记录
export const pagePlaneComponentDefectRecord = (params?: any) =>
	request({
		url: Api.PagePlaneComponentDefectRecord,
		method: 'post',
		data: params,
	});

export const getUploadTemplatePlaneComponentDefectRecord = () => request({
	url: Api.GetUploadTemplatePlaneComponentDefectRecord,
	method: 'get',
	responseType: 'blob'
});
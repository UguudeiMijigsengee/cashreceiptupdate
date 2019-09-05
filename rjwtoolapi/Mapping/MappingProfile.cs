using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Model.Model;
using Model.Model.CashReceipt;
using Model.Model.CashReceipt.Resource;
using Model.Model.RetailLink;
using Model.Model.RetailLink.Resource;

namespace rjwtoolapi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<CashReceiptApplyCheck, CashReceiptApplyCheckResponse>()
                .ForMember(cc => cc.Amount, opt => opt.MapFrom(cr => cr.InvoiceAmt))
                .ForMember(cc => cc.InvoiceNumber, opt => opt.MapFrom(cr => cr.TMWInvoicenumber.Trim()))                
                .ForMember(cc => cc.InvoiceDate, opt => opt.MapFrom(cr => cr.InvoiceDate.ToString("MM/dd/yyyy")));
            CreateMap<RJWLoad, RetailLinkLoadPOValidateRequest>()
                .ForMember(rpv => rpv.poNbr, opt => opt.MapFrom(rj => rj.PO))
                .ForMember(rpv => rpv.poLines, opt => opt.MapFrom(rj => Int32.Parse(rj.POLINES)))
                .ForMember(rpv => rpv.caseQty, opt => opt.MapFrom(rj => rj.CASECOUNT))
                .ForMember(rpv => rpv.weight, opt => opt.MapFrom(rj => rj.WEIGHTORDER.ToString()))
                .ForMember(rpv => rpv.bolNbr, opt => opt.MapFrom(rj => ""))
                .ForMember(rpv => rpv.proNbr, opt => opt.MapFrom(rj => Int32.Parse(rj.PRO)))
                .ForMember(rpv => rpv.weight, opt => opt.MapFrom(rj => rj.WEIGHTORDER.ToString()));

            // API to API Resource
            CreateMap<RetailLinkLoadPOValidateResponse, RetailLinkAvailableAppWindowsRequestShipment>()
                .ForMember(rs => rs.convInd, opt => opt.MapFrom(vr => vr.convInd))
                .ForMember(rs => rs.cutOffDateInStrFmt, opt => opt.MapFrom(vr => DateTime.Parse(vr.cutOffDate).ToString("yyyy-M-d")))
                .ForMember(rs => rs.inventoryTypeId, opt => opt.MapFrom(vr => vr.inventoryTypeId))
                .ForMember(rs => rs.isDummy, opt => opt.MapFrom(vr => false))
                .ForMember(rs => rs.loadTypeId, opt => opt.MapFrom(vr => Int32.Parse(vr.loadType)))
                .ForMember(rs => rs.paymentTerm, opt => opt.MapFrom(vr => vr.paymentTerm))
                .ForMember(rs => rs.poCaseQty, opt => opt.MapFrom(vr => vr.caseQuantity))
                .ForMember(rs => rs.poEvent, opt => opt.MapFrom(vr => vr.poEventCode))
                .ForMember(rs => rs.specialPoType, opt => opt.MapFrom(vr => vr.specialPoType))
                .ForMember(rs => rs.vendorDepartmentNum, opt => opt.MapFrom(vr => "-1"))
                .ForMember(rs => rs.vendorId, opt => opt.MapFrom(vr => vr.vendorId))
                .ForMember(rs => rs.vendorSequenceNum, opt => opt.MapFrom(vr => "-1"));
            CreateMap<RetailLinkLoadPOValidateResponse, RetailLinkApptDeliveryRequestShipment>()
                .ForMember(vr => vr.billOfLading, opt => opt.MapFrom(drs => drs.bolNbr))
                .ForMember(vr => vr.convInd, opt => opt.MapFrom(drs => drs.convInd))
                .ForMember(vr => vr.cutOffDate, opt => opt.MapFrom(drs => DateTime.Parse(drs.cutOffDate).ToString("yyyy-MM-dd")))
                .ForMember(vr => vr.inventoryTypeId, opt => opt.MapFrom(drs => Int32.Parse(drs.inventoryTypeId)))
                .ForMember(vr => vr.isDummy, opt => opt.MapFrom(drs => false))
                .ForMember(vr => vr.loadTypeDets, opt => opt.MapFrom(drs => new RetailLinkApptDeliveryRequestShipmentLoadTypeDets { capacityMeasureId = Int32.Parse(drs.loadType) }))
                .ForMember(vr => vr.loadTypeId, opt => opt.MapFrom(drs => Int32.Parse(drs.loadType)))
                .ForMember(vr => vr.obsoleteIndicator, opt => opt.MapFrom(drs => "N"))
                .ForMember(vr => vr.poAvailableQty, opt => opt.MapFrom(drs => Int32.Parse(drs.availableQty)))
                .ForMember(vr => vr.poCaseQty, opt => opt.MapFrom(drs => Int32.Parse(drs.caseQuantity)))
                .ForMember(vr => vr.poEvent, opt => opt.MapFrom(drs => drs.poEventCode))
                .ForMember(vr => vr.poLines, opt => opt.MapFrom(drs => Int32.Parse(drs.poLineCount)))
                .ForMember(vr => vr.poMabd, opt => opt.MapFrom(drs => DateTime.Parse(drs.cutOffDate).ToString("yyyy-MM-dd")))
                .ForMember(vr => vr.poNumber, opt => opt.MapFrom(drs => drs.poNbr))
                .ForMember(vr => vr.purchaseOrderDets, opt => opt.MapFrom(drs => new RetailLinkApptDeliveryRequestShipmentPurchaseOrderDets { poNumber = drs.poNbr }))
                .ForMember(vr => vr.specialPoType, opt => opt.MapFrom(drs => drs.specialPoType))
                .ForMember(vr => vr.vendorNbr, opt => opt.MapFrom(drs => drs.vendorId))
                .ForMember(vr => vr.weight, opt => opt.MapFrom(drs => drs.weight));
        }
    }
}

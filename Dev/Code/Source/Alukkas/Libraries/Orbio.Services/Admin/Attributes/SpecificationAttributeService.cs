using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public class SpecificationAttributeService : ISpecificationAttributeService
    {
        #region Fields
        private readonly OrbioAdminContext context = new OrbioAdminContext();
        #endregion

        public List<SpecificationAttribute> GetSpecificationAttributes()
        {
            var model = context.SpecificationAttributes.ToList();
            return model;
        }

        public SpecificationAttribute GetSpecificationAttributesById(int Id)
        {
            var model = context.SpecificationAttributes.Where(m => m.Id == Id).FirstOrDefault();
            if (model != null)
            {
                model.SpecificationAttributeOptions = context.SpecificationAttributeOptions.Where(m => m.SpecificationAttributeId == Id).ToList();
            }
            return model;
        }

        public int AddOrUpdate(int Id, string Name, int DisplayOrder)
        {
            try
            {
                var spec = context.SpecificationAttributes.Where(m => m.Id == Id).FirstOrDefault();
                if (spec != null)
                {
                    spec.Name = Name;
                    spec.DisplayOrder = DisplayOrder;
                    context.SaveChanges();
                }
                else
                {
                    var specification = context.SpecificationAttributes.FirstOrDefault();
                    specification.Name = Name;
                    specification.DisplayOrder = DisplayOrder;
                    context.SpecificationAttributes.Add(specification);
                    context.SaveChanges();
                    return specification.Id;
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int DeleteSpecificationAttribute(int Id)
        {
            try
            {
                var spec = context.SpecificationAttributes.Where(m => m.Id == Id).FirstOrDefault();
                if (spec != null)
                {
                    context.SpecificationAttributes.Remove(spec);
                    context.SaveChanges();
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int DeleteSpecificationAttributeOption(int Id)
        {
            try
            {
                var spec = context.SpecificationAttributeOptions.Where(m => m.Id == Id).FirstOrDefault();
                if (spec != null)
                {
                    context.SpecificationAttributeOptions.Remove(spec);
                    context.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public SpecificationAttributeOption GetSpecificationAttributeOptionById(int Id)
        {
            var result = context.SpecificationAttributeOptions.Where(m => m.Id == Id).FirstOrDefault();
            return result;
        }

        public  int AddSpecificationOption(int Id,string Name,int DisplayOrder,int SpecificationAttributeId)
        {
            try
            {
                var result = context.SpecificationAttributeOptions.Where(m => m.Id == Id).FirstOrDefault();
                if (result != null)
                {
                    result.Name = Name;
                    result.DisplayOrder = DisplayOrder;
                    context.SaveChanges();
                }
                else
                {
                    var spec = context.SpecificationAttributeOptions.FirstOrDefault();
                    spec.Name = Name;
                    spec.DisplayOrder = DisplayOrder;
                    spec.SpecificationAttributeId = SpecificationAttributeId;
                    context.SpecificationAttributeOptions.Add(spec);
                    context.SaveChanges();
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}
